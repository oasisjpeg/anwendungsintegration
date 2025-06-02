"use client";

import { useCallback, useState } from "react";
import { Button } from "@heroui/button";
import { Modal, ModalBody, ModalContent, ModalFooter, ModalHeader } from "@heroui/modal";

// Hardcoded article and quiz data matching your JSON structure
const article = {
  articleId: 1,
  title: "Understanding the Duck Curve",
  url: "/images/duck_curve.jpg",
  content:
    "The 'duck curve' describes how the net electricity demand on the grid changes throughout the day as solar power production increases. During midday, solar generation is high, reducing demand from the grid (the 'belly' of the duck). In the evening, as solar production drops and people return home, grid demand rises sharply (the 'neck' of the duck). Managing the duck curve is a major challenge for modern energy systems.",
  quiz: {
    quizId: 2,
    title: "Duck Curve Quiz",
    questions: [
      {
        questionId: 10,
        title: "What does the 'duck curve' represent?",
        answer1: "Electricity demand and solar production over a day",
        answer2: "A type of renewable energy",
        answer3: "Water usage in households",
        answer4: "Battery storage trends"
      },
      {
        questionId: 11,
        title: "When does the steepest rise in the duck curve typically occur?",
        answer1: "Early morning",
        answer2: "Midday",
        answer3: "Evening",
        answer4: "Late night"
      },
      {
        questionId: 12,
        title: "Which of the following helps flatten the duck curve?",
        answer1: "Using more electricity in the evening",
        answer2: "Shifting electricity use to midday",
        answer3: "Building more coal plants",
        answer4: "Turning off solar panels at noon"
      }
    ]
  }
};

// Mock submit function
async function submitUserAnswer({ userId, questionId, selectedAnswer }) {
  // Simulate network delay
  await new Promise((resolve) => setTimeout(resolve, 500));
  // Simulate API POST (replace with real fetch as needed)
  // await fetch('/api/useranswer', { ... })
  return { ok: true };
}

function ProgressLostModal({ isOpen, onClose }) {
  return (
    <div style={{ position: "relative", zIndex: 600 }}>
      <Modal backdrop="blur" isOpen={isOpen} onClose={onClose}>
        <ModalContent className="md-4 pb-5">
          <ModalHeader>
            <h3>Quiz nicht abgeschlossen</h3>
          </ModalHeader>
          <ModalBody>
            <p>
              Wenn du jetzt gehst, geht dein bisheriger Fortschritt verloren!
            </p>
          </ModalBody>
          <ModalFooter>
            <Button color="default" onPress={onClose}>
              Fortsetzen
            </Button>
            <Button
              color="danger"
              onPress={() => {
                onClose();
                window.history.back();
              }}
            >
              Zur Übersicht
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </div>
  );
}

export default function ArticlePage() {
  const [currentIdx, setCurrentIdx] = useState(0);
  const [selected, setSelected] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const [finished, setFinished] = useState(false);
  const [answers, setAnswers] = useState({});
  const [isProgressModalOpen, setIsProgressModalOpen] = useState(false);
  const [pendingNavigation, setPendingNavigation] = useState(null);


  const questions = article.quiz.questions;
  const currentQuestion = questions[currentIdx];

  const handleBack = useCallback(() => {
    if (!finished && currentIdx > 0) {
      setIsProgressModalOpen(true);
      setPendingNavigation(() => () => window.history.back());
    } else {
      window.history.back();
    }
  }, [finished, currentIdx]);

  async function handleContinue() {
    if (selected === null) return;
    setSubmitting(true);
    // Mock userId as 1
    await submitUserAnswer({
      userId: 1,
      questionId: currentQuestion.questionId,
      selectedAnswer: selected + 1 // 1-based index
    });
    setAnswers((prev) => ({
      ...prev,
      [currentQuestion.questionId]: selected
    }));

    console.log('Submitting answer for question:', currentQuestion.questionId, 'Answer:', selected);
    setSubmitting(false);
    setSelected(null);
    if (currentIdx < questions.length - 1) {
      setCurrentIdx((idx) => idx + 1);
    } else {
      setFinished(true);
      console.log('All submissions:', { ...answers, [currentQuestion.questionId]: selected });
    }
  }

  return (
    <main className="min-h-screen bg-white dark:bg-black text-foreground">

      <section className="max-w-2xl mx-auto px-4 py-12 relative">

        <Button
          className="bg-indigo-600 dark:bg-indigo-500 text-white font-semibold rounded-xl px-6 py-2 text-sm shadow-md mt-2 absolute top-4 left-4"
          variant="flat"
          onPress={handleBack}
        >
          Zurück
        </Button>

        <ProgressLostModal
          isOpen={isProgressModalOpen}
          onClose={() => setIsProgressModalOpen(false)}
        />
        <h1 className="text-3xl font-bold pt-6 mb-4">{article.title}</h1>
        <img
          src={article.url}
          alt={article.title}
          className="rounded-2xl mb-6"
        />
        <div className="prose dark:prose-invert mb-8">{article.content}</div>

        <div className="bg-white dark:bg-zinc-900 shadow-lg rounded-2xl p-6 mb-8">
          <h2 className="text-xl font-semibold mb-2">{article.quiz.title}</h2>
          <div className="mb-4 text-gray-600 dark:text-gray-300">
            Frage {currentIdx + 1} von {questions.length}
          </div>
          {!finished ? (
            <>
              <div className="font-medium mb-2">{currentQuestion.title}</div>
              <div className="flex flex-col gap-2 mb-4">
                {[currentQuestion.answer1, currentQuestion.answer2, currentQuestion.answer3, currentQuestion.answer4].map(
                  (opt, i) => (
                    <label key={i} className="flex items-center gap-2">
                      <input
                        type="radio"
                        name={`question-${currentQuestion.questionId}`}
                        value={i}
                        disabled={submitting}
                        checked={selected === i}
                        onChange={() => setSelected(i)}
                        className="accent-indigo-600"
                      />
                      {opt}
                    </label>
                  )
                )}
              </div>
              <Button
                className="bg-indigo-600 dark:bg-indigo-500 text-white font-semibold rounded-xl px-6 py-2 text-sm shadow-md mt-2"
                variant="flat"
                disabled={selected === null || submitting}
                onPress={handleContinue}
              >
                {currentIdx === questions.length - 1 ? "Abschließen" : "Weiter"}
              </Button>
            </>
          ) : (
            <div className="mt-4 text-center">
              <div className="text-lg font-bold">
                Danke für deine Teilnahme!
              </div>
              <div className="text-gray-500 mt-1">
                Du hast {questions.length} Fragen beantwortet.
              </div>
            </div>
          )}
        </div>
      </section>
    </main>
  );
}
