import axios from "axios";

const API_HTTP_URL =
  // @ts-expect-error: Ignorar
  process.env.NODE_ENV === "production"
    ? "https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/api"
    : "/api";

const httpClient = axios.create({
  baseURL: API_HTTP_URL,
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
  },
});

export default httpClient;
