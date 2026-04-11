import { GoogleGenerativeAIEmbeddings } from "@langchain/google-genai";
import { Document } from "@langchain/core/documents";

function cosineSimilarity(vecA: number[], vecB: number[]): number {
  let dotProduct = 0;
  let normA = 0;
  let normB = 0;
  for (let i = 0; i < vecA.length; i++) {
    dotProduct += vecA[i] * vecB[i];
    normA += vecA[i] * vecA[i];
    normB += vecB[i] * vecB[i];
  }
  return dotProduct / (Math.sqrt(normA) * Math.sqrt(normB));
}

export async function semanticChunking(
  text: string,
  embeddingsModel: GoogleGenerativeAIEmbeddings,
  similarityThreshold: number = 0.8
): Promise<Document[]> {
  const sentences = text.match(/[^.!?]+[.!?]+/g) || [text];
  const cleanedSentences = sentences.map(s => s.trim()).filter(s => s.length > 0);

  if (cleanedSentences.length === 0) return [];

  const embeddings = await embeddingsModel.embedDocuments(cleanedSentences);

  const chunks: string[] = [];
  let currentChunk = [cleanedSentences[0]];

  for (let i = 1; i < cleanedSentences.length; i++) {
    const prevEmbedding = embeddings[i - 1];
    const currEmbedding = embeddings[i];

    const similarity = cosineSimilarity(prevEmbedding, currEmbedding);

    if (similarity >= similarityThreshold) {
      currentChunk.push(cleanedSentences[i]);
    } else {
      chunks.push(currentChunk.join(" "));
      currentChunk = [cleanedSentences[i]];
    }
  }
  
  if (currentChunk.length > 0) {
    chunks.push(currentChunk.join(" "));
  }

  return chunks.map(chunk => new Document({ pageContent: chunk }));
}
