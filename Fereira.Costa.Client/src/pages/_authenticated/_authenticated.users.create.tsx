import { createFileRoute } from "@tanstack/react-router";
import { Authorize } from "~/guards/guards";
import { IUserRole } from "~/interfaces/IUserProfileDto";
import { UserForm } from "./-components/user-form";
import { useMutation } from "@tanstack/react-query";
import httpClient from "~/lib/http-client";
import { Routes } from "~/constants/consts";
import { useEffect } from "react";
import { queryClient } from "~/lib/tanstack-query";

export const Route = createFileRoute(
  "/_authenticated/_authenticated/users/create"
)({
  component: Authorize(RouteComponent, [IUserRole.USER]),
});

async function createUserRequest(data: unknown) {
  return httpClient.post(Routes.Users.CreateUser, data);
}

function RouteComponent() {
  const { isPending, mutateAsync } = useMutation({
    mutationFn: createUserRequest,
  });

  return <UserForm isPending={isPending} onSubmitFn={mutateAsync} />;
}
