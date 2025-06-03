// types.ts
export interface Question {
  id: number;
  questionText: string;
  firstAnswerOption: string;
  secondAnswerOption: string;
  thirdAnswerOption: string;
  fourthAnswerOption: string;
  correctAnswerIndex: number;
  quizId: number;
  quiz: null;
}

export interface Quiz {
  id: number;
  title: string;
  articleId: number;
  article: null;
  questions: Question[];
}

export interface Article {
  id: number;
  title: string;
  content: string;
  url: string[];
  dateTime: string;
  description: string;
}

export interface ArticleDetailResponse {
  article: Article;
  quiz: Quiz;
}
