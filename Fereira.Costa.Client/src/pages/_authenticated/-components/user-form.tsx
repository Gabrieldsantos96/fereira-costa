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
import TextInput from "~/components/text-input";
import LocationPicker, { LocationData } from "~/components/location-picker";
import { User, Mail, MapPin, Phone, CalendarIcon } from "lucide-react";
import { useEffect } from "react";
import { MaskedInput } from "~/components/masked-input";
import z from "zod";
import { showToast } from "~/utils/trigger-toast";
import { MessageType } from "~/services/toast-service";
import { handleError } from "~/utils/handle-error";
import { Link } from "@tanstack/react-router";
import { CustomCombobox } from "~/components/ui/custom-combobox";
import { searchMunicipalities, searchNationalities } from "~/lib/thirty-api";
import { signupSchema } from "~/validations/sign-up-schema";
import { Calendar } from "~/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "~/components/ui/popover";
import { cn } from "~/lib/utils";
import { formatDateToShort } from "~/utils/date";

interface UpdateUserFormProps {
  userId?: string;
  onSubmitFn: (data: Record<string, any>) => Promise<unknown>;
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
  const form = useForm<UpdateUserFormData>({
    resolver: zodResolver(updateUserSchema),
    mode: "onChange",
    defaultValues: {
      email: "",
      userName: "",
      cpf: "",
      nationality: "",
      naturalness: "",
      phone: "",
      name: {
        firstName: "",
        lastName: "",
      },
      address: {
        street: "",
        city: "",
        zipcode: "",
        country: "Brasil",
        latitude: 0,
        longitude: 0,
        number: "",
      },
    },
  });

  useEffect(() => {
    if (initialData) {
      const geo = initialData.geo || "LONG--49.295658,LAT--25.499792";
      const parts = geo.split(",");
      const long = parts[0].replace("LONG-", "");
      const lat = parts[1].replace("LAT-", "");
      form.reset({
        id: initialData?.refId,
        email: initialData?.email,
        userName: initialData?.userName,
        nationality: initialData?.nationality,
        naturalness: initialData?.naturalness,
        cpf: initialData?.cpf?.value,
        phone: initialData?.phone,
        birthday: initialData?.birthDay
          ? new Date(initialData.birthDay)
          : undefined,

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
    }
  }, [initialData, form]);

  const onSubmit = async (data: UpdateUserFormData) => {
    try {
      const {
        address: { longitude, latitude, ...partialAddress },
        ...partialData
      } = data;
      const formData = {
        ...partialData,
        ...partialAddress,
        geolocation: `LONG-${longitude},LAT-${latitude}`,
      };

      await onSubmitFn(formData);
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
            <CardTitle className="flex items-center gap-2 sr-only">
              <User className="h-5 w-5" />
              Formulário do usuário
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="email"
                disabled={!!initialData?.email}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>E-mail</FormLabel>
                    <FormControl>
                      <TextInput
                        disabled={!!initialData?.email}
                        startIcon={
                          <Mail className="h-4 w-4 text-muted-foreground" />
                        }
                        placeholder="usuario@exemplo.com"
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
                name="birthday"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <FormLabel>Data de nascimento</FormLabel>
                    <Popover>
                      <PopoverTrigger asChild>
                        <FormControl>
                          <Button
                            variant={"outline"}
                            className={cn(
                              "w-max-[320px] pl-3 text-left font-normal",
                              !field.value && "text-muted-foreground"
                            )}
                          >
                            {field.value ? (
                              formatDateToShort(field.value)
                            ) : (
                              <span>Escolha uma data</span>
                            )}
                            <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                          </Button>
                        </FormControl>
                      </PopoverTrigger>
                      <PopoverContent className="w-auto p-0" align="start">
                        <Calendar
                          mode="single"
                          selected={field.value}
                          onSelect={field.onChange}
                          disabled={(date) =>
                            date > new Date() || date < new Date("1900-01-01")
                          }
                          captionLayout="dropdown"
                        />
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="naturalness"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <FormLabel>Naturalidade</FormLabel>
                    <FormControl>
                      <CustomCombobox
                        value={field.value!}
                        className="w-full"
                        placeholder="Selecione a naturalidade"
                        asyncSearchFn={searchMunicipalities}
                        onSelect={(s) => field.onChange(s)}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="nationality"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <FormLabel>Nacionalidade</FormLabel>
                    <FormControl>
                      <CustomCombobox
                        value={field.value!}
                        className="w-full"
                        placeholder="Selecione a nacionalidade"
                        asyncSearchFn={searchNationalities}
                        onSelect={(s) => field.onChange(s)}
                      />
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
            <div className="grid grid-cols-12 gap-4">
              <FormField
                control={form.control}
                name="address.street"
                render={({ field }) => (
                  <FormItem className="col-span-12 md:col-span-9">
                    <FormLabel>Rua</FormLabel>
                    <FormControl>
                      <TextInput
                        placeholder="Rua das Flores, complemento"
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
                name="address.number"
                render={({ field }) => (
                  <FormItem className="col-span-12 md:col-span-3">
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
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
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
            <div className="space-y-4 border-t pt-4">
              <div className="flex items-center gap-2">
                <MapPin className="w-4 h-4 text-primary" />
                <h4 className="text-sm font-medium">Localização no Mapa</h4>
              </div>
              <p className="text-xs text-muted-foreground">
                Clique no mapa para definir sua localização e preencher
                automaticamente os campos de endereço.
              </p>
              <LocationPicker
                onLocationChange={handleLocationChange}
                initialValueLat={form.watch("address.latitude")}
                initialValueLng={form.watch("address.longitude")}
              />
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <FormField
                  control={form.control}
                  name="address.latitude"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Latitude</FormLabel>
                      <FormControl>
                        <TextInput
                          type="number"
                          step="any"
                          placeholder="Ex: -23.5505"
                          onChange={(e) =>
                            field.onChange(
                              Number.parseFloat(e.target.value) || undefined
                            )
                          }
                          value={field.value?.toString() || ""}
                          readOnly
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="address.longitude"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Longitude</FormLabel>
                      <FormControl>
                        <TextInput
                          type="number"
                          step="any"
                          placeholder="Ex: -46.6333"
                          onChange={(e) =>
                            field.onChange(
                              Number.parseFloat(e.target.value) || undefined
                            )
                          }
                          value={field.value?.toString() || ""}
                          readOnly
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
            </div>
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
            {isPending
              ? "Salvando..."
              : initialData
                ? "Atualizar usuário"
                : "Criar usuário"}
          </Button>
        </div>
      </form>
    </Form>
  );
}
