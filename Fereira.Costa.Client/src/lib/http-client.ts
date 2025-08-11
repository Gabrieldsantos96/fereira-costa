import axios from "axios";

// @ts-expect-error false positive
export const API_HTTP_URL = `${import.meta.env.VITE_API_URL}/api`;

const httpClient = axios.create({
  baseURL: API_HTTP_URL,
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
  },
});

export default httpClient;
