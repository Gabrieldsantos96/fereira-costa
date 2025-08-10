import { useQuery } from "@tanstack/react-query";
import httpClient from "~/lib/http-client";

interface Option {
  value: string;
  label: string;
}

export const useNaturalnessOptions = () => {
  return useQuery<Option[], Error>({
    queryKey: ["naturalnessOptions"],
    queryFn: async () => {
      const response = await httpClient.get(
        "https://servicodados.ibge.gov.br/api/v1/localidades/municipios?orderBy=nome"
      );
      return response.data.map((city: any) => ({
        value: city.nome,
        label: `${city.nome} - ${city.microrregiao.mesorregiao.UF.sigla}`,
      }));
    },
    staleTime: 1000 * 60 * 60 * 24,
    retry: 2,
  });
};
