export const enum IUserRole {
  MANAGER = "MANAGER",
  CLIENT = "CLIENT",
  ADMIN = "ADMIN",
}

export const USER_ROLE: Record<string, string> = {
  MANAGER: "Gerente",
  CLIENT: "Cliente",
  ADMIN: "Administrador",
};

export interface IUserProfileDto {
  id: string;
  refId?: string;
  email: string;
  name: {
    firstName: string;
    lastName: string;
  };
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
