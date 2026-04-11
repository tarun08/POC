import { ChatGoogleGenerativeAI, GoogleGenerativeAIEmbeddings } from "@langchain/google-genai";
import { MemoryVectorStore } from "langchain/vectorstores/memory";
import { PDFLoader } from "@langchain/community/document_loaders/fs/pdf";
import { StateGraph, END, START, Annotation } from "@langchain/langgraph";
import { semanticChunking } from "./semanticChunker";
import { StringOutputParser } from "@langchain/core/output_parsers";
import { ChatPromptTemplate } from "@langchain/core/prompts";

export const GraphState = Annotation.Root({
  question: Annotation<string>(),
  context: Annotation<string>(),
  answer: Annotation<string>(),
});

export class RAGSystem {
  private vectorStore?: MemoryVectorStore;
  private embeddings: GoogleGenerativeAIEmbeddings;
  private llm: ChatGoogleGenerativeAI;
  public graph: any;

  constructor() {
    this.embeddings = new GoogleGenerativeAIEmbeddings({ model: "text-embedding-004" });
    this.llm = new ChatGoogleGenerativeAI({ model: "gemini-1.5-flash", temperature: 0 });
  }

  async ingestPDF(filePath: string) {
    console.log(`Loading PDF from ${filePath}...`);
    const loader = new PDFLoader(filePath);
    const rawDocs = await loader.load();
    const text = rawDocs.map(d => d.pageContent).join(" ");

    console.log(`Applying semantic chunking...`);
    const chunks = await semanticChunking(text, this.embeddings, 0.75);
    console.log(`Created ${chunks.length} semantic chunks.`);

    console.log(`Storing chunks in vector database...`);
    this.vectorStore = await MemoryVectorStore.fromDocuments(chunks, this.embeddings);
    console.log(`Ingestion complete!`);

    this.buildGraph();
  }

  private buildGraph() {
    const retrieveNode = async (state: typeof GraphState.State) => {
      if (!this.vectorStore) throw new Error("Vector store not initialized.");
      const docs = await this.vectorStore.similaritySearch(state.question, 4);
      const context = docs.map(d => d.pageContent).join("\n\n");
      return { context };
    };

    const generateNode = async (state: typeof GraphState.State) => {
      const prompt = ChatPromptTemplate.fromTemplate(`
        You are an assistant for question-answering tasks. 
        Use the following pieces of retrieved context to answer the question. 
        If you don't know the answer, just say that you don't know. 
        Use three sentences maximum and keep the answer concise.
        
        Question: {question} 
        Context: {context} 
        Answer:
      `);
      
      const chain = prompt.pipe(this.llm as any).pipe(new StringOutputParser());
      const answer = await chain.invoke({
        question: state.question,
        context: state.context,
      });

      return { answer };
    };

    const workflow = new StateGraph(GraphState)
      .addNode("retrieve", retrieveNode)
      .addNode("generate", generateNode)
      .addEdge(START, "retrieve")
      .addEdge("retrieve", "generate")
      .addEdge("generate", END);

    this.graph = workflow.compile();
  }

  async ask(question: string) {
    if (!this.graph) throw new Error("Graph not initialized. Did you ingest data?");
    const result = await this.graph.invoke({ question });
    return result.answer;
  }
}
