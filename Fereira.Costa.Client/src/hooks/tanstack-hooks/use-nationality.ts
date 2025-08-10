import { useQuery } from "@tanstack/react-query";
import httpClient from "~/lib/http-client";

interface Option {
  value: string;
  label: string;
}

export const useNationalityOptions = () => {
  return useQuery<Option[], Error>({
    queryKey: ["nationalityOptions"],
    queryFn: async () => {
      const response = await httpClient.get(
        "https://restcountries.com/v3.1/all?fields=name"
      );
      return response.data
        .map((country: any) => ({
          value: country.name.common,
          label: country.name.common,
        }))
        .sort((a: Option, b: Option) => a.label.localeCompare(b.label));
    },
  });
};
