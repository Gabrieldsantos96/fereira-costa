using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Fereira.Costa.Domain.ValueObjects;

[Owned]
public record Cpf
{
    [MaxLength(11)]
    public string Value { get; init; }

    public Cpf(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("O valor do CPF não pode ser nulo ou vazio.");
        }

        value = RemoveMask(value);

        if (!IsValid(value))
        {
            throw new ArgumentException($"CPF inválido. CPF: {value}");
        }

        Value = value;
    }

    public static string RemoveMask(string valor)
    {
        return valor.Replace(".", "").Replace("-", "");
    }

    public override string ToString()
    {
        return Value;
    }

    private bool IsValid(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        for (int j = 0; j < 10; j++)
            if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                return false;

        int[] multiplicadores1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicadores2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        string digito = resto.ToString();
        tempCpf += digito;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }
}