import { SearchResponse } from "~/hooks/tanstack-hooks/use-infinite-scroll";
import httpClient from "./http-client";
import { ComboBoxItemType } from "~/components/ui/custom-combobox";

export const searchMunicipalities = async (
  searchTerm: string,
  page: number,
  pageSize: number
): Promise<SearchResponse<ComboBoxItemType>> => {
  try {
    const response = await httpClient.get(
      "https://servicodados.ibge.gov.br/api/v1/localidades/municipios?orderBy=nome"
    );

    let fullData = response.data.map((city: any) => ({
      value: `${city.nome} - ${city.microrregiao?.nome}`,
      label: `${city.nome} - ${city.microrregiao?.nome}`,
    }));

    if (searchTerm) {
      fullData = fullData.filter((item: ComboBoxItemType) =>
        item.label.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    const start = (page - 1) * pageSize;
    const paginatedData = fullData.slice(start, start + pageSize);
    const totalItems = fullData.length;

    return {
      data: paginatedData,
      hasMore: start + pageSize < totalItems,
      total: totalItems,
    };
  } catch (error) {
    console.error("Error fetching municipalities:", error);
    return {
      data: [],
      hasMore: false,
      total: 0,
    };
  }
};

export const searchNationalities = async (
  searchTerm: string,
  page: number,
  pageSize: number
): Promise<SearchResponse<ComboBoxItemType>> => {
  try {
    const response = await httpClient.get(
      "https://restcountries.com/v3.1/all?fields=name"
    );

    let fullData = response.data.map((item: any) => ({
      value: `${item?.name?.common} - ${item?.name?.official}`,
      label: `${item?.name?.common} - ${item?.name?.official}`,
    }));

    if (searchTerm) {
      fullData = fullData.filter((item: ComboBoxItemType) =>
        item.label.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    const start = (page - 1) * pageSize;
    const paginatedData = fullData.slice(start, start + pageSize);
    const totalItems = fullData.length;

    return {
      data: paginatedData,
      hasMore: start + pageSize < totalItems,
      total: totalItems,
    };
  } catch (error) {
    console.error("Error fetching municipalities:", error);
    return {
      data: [],
      hasMore: false,
      total: 0,
    };
  }
};
