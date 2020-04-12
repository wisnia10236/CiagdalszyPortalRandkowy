import { Photo } from "./photo";

export interface User {
  /** Podst Inf */
  id: number;
  username: string;
  gender: string;
  age: number;
  zodiacSign: string;
  created: Date;
  lastActive: Date;
  city: string;
  country: string;
  /** Zakładka info */
  growth: string;
  eyeColor: string;
  hairColor: string;
  martialStatus: string;
  education: string;
  profession: string;
  children: string;
  languages: string;
  /** Zakładka o mnie */
  motto: string;
  description: string;
  personality: string;
  lookingFor: string;
  /** Zakładka Pasje, zaint */
  interests: string;
  freeTime: string;
  sport: string;
  movies: string;
  music: string;
  /** Zakładka Preferencje  */
  iLike: string;
  idoNotLike: string;
  makesMeLaugh: string;
  itFeelsBestIn: string;
  friendeWouldDescribeMe: string;
  /** Zakładka Zdjęcia */
  photos: Photo[];
  photoUrl: string;
}
