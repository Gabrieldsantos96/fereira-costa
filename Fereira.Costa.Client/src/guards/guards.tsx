import { useSession } from "~/contexts/session-provider";
import type { IUserRole } from "~/interfaces/IUserProfileDto";
import { Navigate } from "@tanstack/react-router";

export function Authorize(
  Component: React.ComponentType<any>,
  allowedRoles: IUserRole[]
) {
  return function Guard(props: any) {
    const { applicationUser } = useSession();

    if (!applicationUser) {
      return <Navigate to="/401" />;
    }

    return <Component {...props} />;
  };
}
