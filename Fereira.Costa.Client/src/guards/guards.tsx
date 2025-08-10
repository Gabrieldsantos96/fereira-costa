import { useSession } from "~/contexts/session-provider";
import type { IUserRole } from "~/interfaces/IUserProfileDto";

export function Authorize(
  Component: React.ComponentType<any>,
  allowedRoles: IUserRole[]
) {
  return function Guard(props: any) {
    const { applicationUser } = useSession();

    return <Component {...props} />;
  };
}
