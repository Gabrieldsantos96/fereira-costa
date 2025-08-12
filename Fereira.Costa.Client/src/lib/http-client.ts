import axios from "axios";

// @ts-expect-error: Property 'env' does not exist on type 'ImportMeta'
const API_HTTP_URL = import.meta.env.VITE_API_URL;

const httpClient = axios.create({
  baseURL: API_HTTP_URL,
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
  },
});

export default httpClient;
