import { ValidationMessages } from "~/utils/validation-messages";
import { z } from "zod";

const today = new Date();

export const signupSchema = z
  .object({
    id: z.string().optional(),
    birthday: z
      .date(
        ValidationMessages.FieldRequired.replace(
          "{field}",
          "Data de nascimento"
        )
      )
      .refine((date) => date <= today, {
        message: ValidationMessages.FieldValueInvalid.replace(
          "{field}",
          "Data de nascimento não pode ser no futuro"
        ),
      }),
    naturalness: z.string().optional(),
    nationality: z.string().optional(),
    email: z
      .string()
      .nonempty(ValidationMessages.FieldRequired.replace("{field}", "email"))
      .email(ValidationMessages.FieldValueInvalid.replace("{field}", "email"))
      .max(256, ValidationMessages.MaxChars.replace("{max}", "256")),
    userName: z
      .string()
      .nonempty(
        ValidationMessages.FieldRequired.replace("{field}", "nome de usuário")
      )
      .max(256, ValidationMessages.MaxChars.replace("{max}", "256")),
    password: z
      .string()
      .nonempty(ValidationMessages.FieldRequired.replace("{field}", "senha")),
    confirmPassword: z
      .string()
      .nonempty(
        ValidationMessages.FieldRequired.replace("{field}", "confirmação")
      ),
    name: z.object({
      firstName: z
        .string()
        .nonempty(
          ValidationMessages.FieldRequired.replace("{field}", "primeiro nome")
        )
        .max(
          100,
          ValidationMessages.MaxChars.replace(
            "{field}",
            "primeiro nome"
          ).replace("{max}", "100")
        ),
      lastName: z
        .string()
        .nonempty(
          ValidationMessages.FieldRequired.replace("{field}", "sobrenome")
        )
        .max(
          100,
          ValidationMessages.MaxChars.replace("{field}", "sobrenome").replace(
            "{max}",
            "100"
          )
        ),
    }),
    cpf: z
      .string()
      .nonempty(ValidationMessages.FieldRequired.replace("{field}", "CPF")),
    address: z.object({
      street: z
        .string()
        .nonempty(ValidationMessages.FieldRequired.replace("{field}", "rua")),
      city: z
        .string()
        .nonempty(
          ValidationMessages.FieldRequired.replace("{field}", "cidade")
        ),
      number: z
        .string()
        .nonempty(
          ValidationMessages.FieldRequired.replace("{field}", "número")
        ),
      zipcode: z
        .string()
        .nonempty(ValidationMessages.FieldRequired.replace("{field}", "CEP")),
      country: z
        .string()
        .nonempty(ValidationMessages.FieldRequired.replace("{field}", "país")),
      latitude: z.number(
        ValidationMessages.FieldRequired.replace("{field}", "latitude")
      ),
      longitude: z.number(
        ValidationMessages.FieldRequired.replace("{field}", "longitude")
      ),
    }),
    phone: z
      .string()
      .nonempty(ValidationMessages.FieldRequired.replace("{field}", "telefone"))
      .max(20, ValidationMessages.MaxChars.replace("{max}", "20")),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: ValidationMessages.FieldValueInvalid.replace(
      "{field}",
      "confirmação"
    ),
    path: ["confirmPassword"],
  });

export type SignupFormData = z.infer<typeof signupSchema>;
