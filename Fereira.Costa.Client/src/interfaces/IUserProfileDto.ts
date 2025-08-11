export const enum IUserRole {
  USER = "USER",
}

export const USER_ROLE: Record<string, string> = {
  USER: "Usuário",
};

export interface IUserProfileDto {
  id: string;
  refId?: string;
  email: string;
  name: {
    firstName: string;
    lastName: string;
  };
  birthDay: string;
  nationality: string;
  naturalness: string;
  cpf: { value: string };
  userName: string;
  phone: string;
  role?: IUserRole;
  createdAt: string;
  updatedAt?: string;
  address: {
    city: string;
    street: string;
    zipcode: string;
    number: string;
    geo: string;
  };
}
