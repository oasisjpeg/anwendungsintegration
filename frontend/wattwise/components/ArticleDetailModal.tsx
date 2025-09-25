"use client";

import { useState } from "react";
import axios from "axios";
import { Article, Quiz, Question } from "@/types/types";

interface ArticleDetail201ModalProps {
  article: Article | null;
  quiz: Quiz | null;
  onClose: () => void;
}

export function ArticleDetailModal({ article, quiz, onClose }: ArticleDetail201ModalProps) {
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [selectedAnswers, setSelectedAnswers] = useState<Record<number, number | null>>({});
  const [showScore, setShowScore] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  if (!article || !quiz) return null;

  const currentQuestion = quiz.questions[currentQuestionIndex];

  const handleAnswerSelect = (questionId: number, answerIndex: number) => {
    setSelectedAnswers(prev => ({ ...prev, [questionId]: answerIndex }));
  };

  const submitAnswer = async () => {
    const answerIndex = selectedAnswers[currentQuestion.id];
    if (answerIndex === undefined || answerIndex === null) {
      alert("Bitte wähle eine Antwort aus.");
      return;
    }

    try {
      setSubmitting(true);

      // Get the user's auth token (adjust as needed for your auth setup)
      const token = localStorage.getItem("token"); // or from context, etc.

      if (!token) {
        alert("Bitte einloggen, um das Quiz abzuschicken.");
        return;
      }

      // Prepare the request body according to your DTO
      const requestBody = {
        QuestionId: currentQuestion.id,
        AnswerSelectionIndex: answerIndex,
        // quizId: quiz.id, // Add this if your backend expects it (not in your DTO)
      };

      const response = await axios.post(
        `http://localhost:5137/api/articles/${article.id}/quiz/submissions`,
        requestBody,
        {
          headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
          },
        }
      );

      // If status is 200, quiz is finished (next question is the last)
      if (response.status === 200) {
        setShowScore(true);
      } else if (response.status === 202) {
        // If status is 202, proceed to next question
        if (currentQuestionIndex < quiz.questions.length - 1) {
          setCurrentQuestionIndex(currentQuestionIndex + 1);
        } else {
          setShowScore(true);
        }
      }
    } catch (error) {
      console.error("Fehler beim Absenden der Antwort:", error);
      alert("Fehler beim Absenden der Antwort.");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4 pb-20">
      <div className="bg-white dark:bg-zinc-900 rounded-2xl p-6 max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        <h2 className="text-2xl font-semibold mb-4">{article.title}</h2>
        <p className="text-gray-600 dark:text-gray-300 mb-2">{article.description}</p>
        {article.url && article.url.length > 0 && (
          <img
            src={article.url[0]}
            alt={article.title}
            className="rounded201-lg object-cover mb-4 w-full max-h-64"
          />
        )}
        <p className="text-gray-700 dark:text-gray-200 mb-6">{article.content}</p>

        <div className="mt-6">
          <h3 className="text-xl font-semibold mb-3">{quiz.title}</h3>

          {!showScore ? (
            <div className="space-y-4">
              <div key={currentQuestion.id} className="bg-gray-50 dark:bg-zinc-800 rounded-xl p-4">
                <h4 className="font-medium mb-2">
                  Frage {currentQuestionIndex + 1} von {quiz.questions.length}: {currentQuestion.questionText}
                </h4>
                <ul className="space-y-2">
                  {[
                    currentQuestion.firstAnswerOption,
                    currentQuestion.secondAnswerOption,
                    currentQuestion.thirdAnswerOption,
                    currentQuestion.fourthAnswerOption
                  ].map((option, index) => (
                    <li key={index}>
                      <label className="flex items-center gap-2">
                        <input
                          type="radio"
                          name={`question-${currentQuestion.id}`}
                          checked={selectedAnswers[currentQuestion.id] === index}
                          onChange={() => handleAnswerSelect(currentQuestion.id, index)}
                          disabled={submitting}
                        />
                        {option}
                      </label>
                    </li>
                  ))}
                </ul>
              </div>
              <button
                onClick={submitAnswer}
                disabled={selectedAnswers[currentQuestion.id] === undefined || submitting}
                className="mt-4 bg-indigo-600 dark:bg-indigo-500 text-white font201-semibold rounded-xl px-4 py-2 text-sm shadow-md hover:bg-indigo-700 disabled:opacity-50"
              >
                {submitting ? "Wird gespeichert..." :
                  currentQuestionIndex < quiz.questions.length - 1 ? "Nächste Frage" : "Ergebnis anzeigen"}
              </button>
            </div>
          ) : (
            <div className="space-y-4">
              <p className="text-lg font-semibold">Quiz abgeschlossen!</p>
              <div className="space-y-3">
                {quiz.questions.map((q, qIndex) => {
                  const userAnswerIndex = selectedAnswers[q.id];
                  const answerOptions = [
                    q.firstAnswerOption,
                    q.secondAnswerOption,
                    q.thirdAnswerOption,
                    q.fourthAnswerOption
                  ];
                  const userAnswer = userAnswerIndex !== null && userAnswerIndex !== undefined
                    ? answerOptions[userAnswerIndex]
                    : "Keine Antwort";
                  const correctAnswer = answerOptions[q.correctAnswerIndex];
                  const isCorrect = userAnswerIndex === q.correctAnswerIndex;

                  return (
                    <div key={q.id} className="bg-gray-50 dark:bg-zinc-800 rounded-xl p-4">
                      <h4 className="font-medium mb-2">
                        Frage {qIndex + 1}: {q.questionText}
                      </h4>
                      <div className="text-sm text-gray-700 dark:text-gray-300">
                        <div>Deine Antwort: <span className={isCorrect ? "text-green-600" : "text-red-600"}>
                          {userAnswer}
                        </span></div>
                        <div>Richtige Antwort: <span className="text-green-600">{correctAnswer}</span></div>
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          )}
        </div>

        <div className="flex justify-end mt-6">
          <button
            onClick={onClose}
            className="bg-indigo-600 dark:bg-indigo-500 text-white font-semibold rounded-xl px-4 py-2 text-sm shadow-md hover:bg-indigo-700"
          >
            Schließen
          </button>
        </div>
      </div>
    </div>
  );
}
