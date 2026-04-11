import "dotenv/config";
import * as readline from "readline/promises";
import { stdin as input, stdout as output } from "process";
import { RAGSystem } from "./rag";
import fs from "fs";

async function main() {
  const rs = new RAGSystem();
  const rl = readline.createInterface({ input, output });

  console.log("==========================================");
  console.log(" Welcome to RAG POC CLI");
  console.log("==========================================\n");
  
  let pdfPath = await rl.question("Enter path to PDF file (e.g. sample.pdf): ");
  
  if (!fs.existsSync(pdfPath)) {
    console.error(`File not found at: ${pdfPath}`);
    process.exit(1);
  }

  try {
    await rs.ingestPDF(pdfPath);
  } catch (error) {
    console.error("Error during ingestion:", error);
    process.exit(1);
  }

  console.log("\nReady! Ask your questions (type 'exit' to quit).");

  while (true) {
    const question = await rl.question("\n> ");
    if (question.toLowerCase() === "exit") break;
    if (!question.trim()) continue;

    try {
      const answer = await rs.ask(question);
      console.log(`\nAnswer: ${answer}`);
    } catch (e) {
      console.error("Error generating answer:", e);
    }
  }

  rl.close();
}

main().catch(console.error);
