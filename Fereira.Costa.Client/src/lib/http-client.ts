import axios from "axios";

const API_HTTP_URL =
  "https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/api";

const httpClient = axios.create({
  baseURL: API_HTTP_URL,
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
  },
});

export default httpClient;
