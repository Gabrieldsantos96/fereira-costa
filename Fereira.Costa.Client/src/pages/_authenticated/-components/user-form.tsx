"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "~/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "~/components/ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "~/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "~/components/ui/select";
import TextInput from "~/components/text-input";
import LocationPicker, { LocationData } from "~/components/location-picker";
import { SignupFormData, signupSchema } from "~/validations/sign-up-schema";
import { User, Mail, MapPin, Phone } from "lucide-react";
import { Badge } from "~/components/ui/badge";
import { useEffect } from "react";
import { MaskedInput } from "~/components/masked-input";
import z from "zod";
import { showToast } from "~/utils/trigger-toast";
import { MessageType } from "~/services/toast-service";
import { handleError } from "~/utils/handle-error";
import { Link } from "@tanstack/react-router";
import { useNaturalnessOptions } from "~/hooks/tanstack-hooks/use-naturalness";
import { useNationalityOptions } from "~/hooks/tanstack-hooks/use-nationality";

interface UpdateUserFormProps {
  userId?: string;
  onSubmitFn: (data: SignupFormData) => Promise<unknown>;
  isPending: boolean;
  initialData?: Record<string, any>;
}

const updateUserSchema = signupSchema.omit({
  password: true,
  confirmPassword: true,
});

export type UpdateUserFormData = z.infer<typeof updateUserSchema>;

export function UserForm({
  onSubmitFn,
  isPending,
  initialData,
}: UpdateUserFormProps) {
  const { data: naturalnessOptions, isLoading: isLoadingNaturalness } =
    useNaturalnessOptions();
  const { data: nationalityOptions, isLoading: isLoadingNationality } =
    useNationalityOptions();

  const form = useForm<UpdateUserFormData>({
    resolver: zodResolver(updateUserSchema),
  });

  useEffect(() => {
    const geo = "LONG--49.295658,LAT--25.499792";
    const parts = geo.split(",");
    const long = parts[0].replace("LONG-", "");
    const lat = parts[1].replace("LAT-", "");
    form.reset({
      id: initialData?.refId,
      email: initialData?.email,
      userName: initialData?.userName,
      birthday: initialData?.birthDay,
      nationality: initialData?.nationality,
      naturalness: initialData?.naturalness,
      phone: initialData?.phone,
      cpf: initialData?.cpf,
      name: {
        firstName: initialData?.name?.firstName,
        lastName: initialData?.name?.lastName,
      },
      address: {
        street: initialData?.address?.street,
        number: initialData?.address?.number,
        city: initialData?.address?.city,
        zipcode: initialData?.address?.zipcode,
        country: initialData?.address?.country || "Brasil",
        latitude: Number(lat),
        longitude: Number(long),
      },
    });
  }, [initialData]);

  const onSubmit = async (data: SignupFormData) => {
    try {
      await onSubmitFn(data);
      showToast({
        text: "Usuário salvo com sucesso",
        type: MessageType.Success,
      });
    } catch (error) {
      handleError(error);
    }
  };

  const handleLocationChange = (locationData: LocationData): void => {
    form.setValue("address.latitude", locationData.lat);
    form.setValue("address.longitude", locationData.lng);

    if (locationData.address) {
      if (locationData.address.street) {
        form.setValue("address.street", locationData.address.street);
      }
      if (locationData.address.houseNumber) {
        form.setValue("address.number", locationData.address.houseNumber);
      }

      if (locationData.address.city) {
        form.setValue("address.city", locationData.address.city);
      }

      if (locationData.address.zipCode) {
        form.setValue("address.zipcode", locationData.address.zipCode);
      }
      if (locationData.address.country) {
        form.setValue("address.country", locationData.address.country);
      }
    }
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <User className="h-5 w-5" />
              Dados Pessoais
              <Badge className="bg-destructive">Inativo</Badge>
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="email"
                disabled
                render={() => (
                  <FormItem>
                    <FormLabel>E-mail</FormLabel>
                    <FormControl>
                      <TextInput
                        disabled
                        startIcon={
                          <Mail className="h-4 w-4 text-muted-foreground" />
                        }
                        placeholder="usuario~exemplo.com"
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="userName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Username</FormLabel>
                    <FormControl>
                      <TextInput
                        startIcon={
                          <User className="h-4 w-4 text-muted-foreground" />
                        }
                        placeholder="usuario123"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="name.firstName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Nome</FormLabel>
                    <FormControl>
                      <TextInput
                        placeholder="João"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="name.lastName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Sobrenome</FormLabel>
                    <FormControl>
                      <TextInput
                        placeholder="Silva"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="cpf"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>CPF</FormLabel>
                    <FormControl>
                      <MaskedInput
                        placeholder="000.000.000-00"
                        mask="999.999.999-99"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="naturalness"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Naturalidade</FormLabel>
                    <FormControl>
                      <Select
                        onValueChange={field.onChange}
                        value={field.value}
                        disabled={isLoadingNaturalness}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Selecione uma cidade" />
                        </SelectTrigger>
                        <SelectContent>
                          {naturalnessOptions?.map((option) => (
                            <SelectItem key={option.value} value={option.value}>
                              {option.label}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="nationality"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Nacionalidade</FormLabel>
                    <FormControl>
                      <Select
                        onValueChange={field.onChange}
                        value={field.value}
                        disabled={isLoadingNationality}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Selecione uma nacionalidade" />
                        </SelectTrigger>
                        <SelectContent>
                          {nationalityOptions?.map((option) => (
                            <SelectItem key={option.value} value={option.value}>
                              {option.label}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <MapPin className="h-5 w-5" />
              Endereço & Localização
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div className="md:col-span-2">
                <FormField
                  control={form.control}
                  name="address.street"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Rua</FormLabel>
                      <FormControl>
                        <TextInput
                          placeholder="Rua das Flores"
                          onChange={field.onChange}
                          value={field.value}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>

              <FormField
                control={form.control}
                name="address.number"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Número</FormLabel>
                    <FormControl>
                      <TextInput
                        placeholder="123"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <FormField
                control={form.control}
                name="address.city"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Cidade</FormLabel>
                    <FormControl>
                      <TextInput
                        placeholder="São Paulo"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="address.zipcode"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>CEP</FormLabel>
                    <FormControl>
                      <MaskedInput
                        placeholder="01234-567"
                        mask="99999-999"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="address.country"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>País</FormLabel>
                    <FormControl>
                      <TextInput
                        placeholder="Brasil"
                        onChange={field.onChange}
                        value={field.value}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <LocationPicker
              onLocationChange={handleLocationChange}
              initialValueLat={form.watch("address.latitude")}
              initialValueLng={form.watch("address.longitude")}
            />
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Phone className="h-5 w-5" />
              Contato & Configurações
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <FormField
              control={form.control}
              name="phone"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Telefone</FormLabel>
                  <FormControl>
                    <MaskedInput
                      mask="(99) 99999-9999"
                      placeholder="(11) 99999-9999"
                      onChange={field.onChange}
                      value={field.value}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </CardContent>
        </Card>

        <div className="flex justify-end gap-4">
          <Link to="/users" search={{ pageSize: 10, searchTerm: "", skip: 0 }}>
            <Button type="button" variant="outline" disabled={isPending}>
              Cancelar
            </Button>
          </Link>
          <Button type="submit" disabled={isPending}>
            {isPending ? "Salvando..." : "Atualizar Usuário"}
          </Button>
        </div>
      </form>
    </Form>
  );
}
