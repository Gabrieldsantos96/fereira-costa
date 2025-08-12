import {
  createFileRoute,
  Link,
  useNavigate,
  useRouter,
} from "@tanstack/react-router";
import { IUserProfileDto, IUserRole } from "~/interfaces/IUserProfileDto";
import { ColumnDef } from "@tanstack/react-table";
import { Button } from "~/components/ui/button";
import {
  ArrowUpDown,
  Edit,
  Loader2,
  MapPin,
  Phone,
  Search,
  Trash2,
  User,
} from "lucide-react";
import { DataTable } from "~/components/data-table";
import { TablePagination } from "~/components/table-pagination";
import { useDeleteUser, useUsers } from "~/hooks/tanstack-hooks/use-user";
import { Input } from "~/components/ui/input";
import { Authorize } from "~/guards/guards";
import z from "zod";
import { Card, CardContent, CardHeader, CardTitle } from "~/components/ui/card";
import { DestructiveDialog } from "~/components/destructive-dialog";
import { openDialog } from "~/utils/trigger-dialog";
import { MessageType } from "~/services/toast-service";
import { showToast } from "~/utils/trigger-toast";
import { applyMask } from "~/utils/apply-mask";
import { useDebounce } from "~/hooks/use-debouce";

const userSearchSchema = z.object({
  searchTerm: z.string().catch(""),
  skip: z.number().catch(0),
  pageSize: z.number().catch(10),
});

export const Route = createFileRoute("/_authenticated/_authenticated/users/")({
  validateSearch: userSearchSchema,
  component: Authorize(RouteComponent, [IUserRole.USER]),
});

function RouteComponent() {
  const { searchTerm, skip, pageSize } = Route.useSearch();

  const navigate = useNavigate();

  const router = useRouter();

  const values = useDebounce({ searchTerm, skip, pageSize }, 500);

  const {
    data: result,
    isFetching,
    isError,
  } = useUsers(values.skip, values.pageSize, values.searchTerm);

  const { mutateAsync } = useDeleteUser();

  const users = result?.data || [];

  const filteredUsers = users.filter(
    (user: IUserProfileDto) =>
      user?.userName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user?.name?.firstName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user?.name?.firstName?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleDelete = async (id: string) => {
    try {
      const result = await openDialog(DestructiveDialog, {
        componentProps: {
          message: "Deseja confirmar exclusão do usuário?",
          variant: "destructive",
        },
      });

      if (result) {
        await mutateAsync(id);
        showToast({
          text: "Usuário excluído com sucesso",
          type: MessageType.Success,
        });
      }
    } catch (error) {
      showToast({
        type: MessageType.Danger,
        text: "Erro ao tentar excluir usuário. Por favor, tente novamente mais tarde.",
      });
    }
  };

  function handlePageChange(page: number) {
    navigate({
      search: { skip: (page - 1) * pageSize, pageSize, searchTerm },
    } as never);
  }

  function handlePageSizeChange(newPageSize: number) {
    navigate({
      search: { pageSize: newPageSize, skip: 0, searchTerm },
    } as never);
  }

  function handleSearchTermChange(newSearchTerm: string) {
    navigate({
      search: { searchTerm: newSearchTerm, skip: 0, pageSize },
    } as never);
  }

  const columns: ColumnDef<IUserProfileDto>[] = [
    {
      accessorKey: "name",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Nome
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const name = row.original.name;
        return (
          <div className="flex items-center gap-3">
            <div className="h-10 w-10 flex items-center justify-center rounded-full bg-accent">
              {name.firstName.charAt(0)}
            </div>
            <div>
              <div className="font-medium">
                {`${name.firstName} ${name.lastName}`}
              </div>
              <div className="text-sm text-muted-foreground">
                {row.original.email}
              </div>
            </div>
          </div>
        );
      },
    },
    {
      accessorKey: "userName",
      header: "Username",
      cell: ({ row }) => (
        <div className="font-mono">{row.getValue("userName")}</div>
      ),
    },
    {
      accessorKey: "phone",
      header: "Telefone",
      cell: ({ row }) => {
        return (
          <div className="flex items-center gap-2">
            <Phone className="h-4 w-4 text-muted-foreground" />
            <div>
              <div className="font-medium">{}</div>
              <div className="text-sm text-muted-foreground">
                {applyMask(row.original.phone, "(99) 99999-9999")}
              </div>
            </div>
          </div>
        );
      },
    },
    {
      accessorKey: "address",
      header: "Endereço",
      cell: ({ row }) => {
        return (
          <div className="flex items-center gap-2">
            <MapPin className="h-4 w-4 text-muted-foreground" />
            <div>
              <div className="font-medium">{}</div>
              <div className="text-sm text-muted-foreground">
                {row.original.address.street},{row.original.address.number}
              </div>
            </div>
          </div>
        );
      },
    },
    {
      accessorKey: "createdAt",
      header: "Criado em",
      cell: ({ row }) => {
        const date = new Date(row.getValue("createdAt"));
        return <div>{date.toLocaleDateString("pt-BR")}</div>;
      },
    },
    {
      id: "actions",
      enableHiding: false,
      cell: ({ row }) => {
        return (
          <div className="flex items-center gap-2">
            <Link
              to="/users/edit/$userId"
              params={{ userId: row.original.refId! }}
            >
              <Button
                variant="ghost"
                size="sm"
                className="h-8 w-8 p-0 hover:bg-blue-50 hover:text-blue-600"
                aria-label={`Editar usuário ${row.original.name.firstName} ${row.original.name.lastName}`}
              >
                <Edit className="h-4 w-4" />
              </Button>
            </Link>

            <Button
              variant="ghost"
              size="sm"
              className="h-8 w-8 p-0 hover:bg-red-50 hover:text-red-600"
              onClick={(e) => {
                e.stopPropagation();
                handleDelete(row.original.refId!);
              }}
              aria-label={`Excluir usuário ${row.original.name.firstName} ${row.original.name.lastName}`}
            >
              <Trash2 className="h-4 w-4" />
            </Button>
          </div>
        );
      },
    },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Usuários</h1>
          <p className="text-muted-foreground">
            Gerencie os usuários do sistema
          </p>
        </div>
      </div>

      <Card>
        <CardHeader className="flex items-center justify-between">
          <CardTitle>Lista de Usuários</CardTitle>
          <Link to="/users/create">
            <Button>Adicionar usuário</Button>
          </Link>
        </CardHeader>
        <CardContent className="p-0">
          <div className="p-6 pb-4">
            <div className="flex items-center space-x-2">
              <div className="relative flex-1 max-w-sm">
                <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
                <Input
                  placeholder="Buscar usuários..."
                  value={searchTerm}
                  onChange={(e) => handleSearchTermChange(e.target.value)}
                  className="pl-8"
                />
              </div>
            </div>
          </div>

          {isFetching ? (
            <div className="flex items-center justify-center py-8">
              <Loader2 className="h-8 w-8 animate-spin" />
              <span className="ml-2">Carregando usuários...</span>
            </div>
          ) : filteredUsers.length === 0 ? (
            <div className="text-center py-8">
              <User className="h-12 w-12 mx-auto mb-4 text-muted-foreground" />
              <h3 className="text-lg font-semibold mb-2">
                {searchTerm
                  ? "Nenhum usuário encontrado"
                  : "Nenhum usuário cadastrado"}
              </h3>
              <p className="text-muted-foreground mb-4">
                {searchTerm
                  ? "Tente ajustar os filtros de busca"
                  : "Comece criando seu primeiro usuário"}
              </p>
            </div>
          ) : isError ? (
            <div className="flex items-center justify-center min-h-[400px]">
              <div className="text-center">
                <h3 className="text-lg font-semibold text-red-600">
                  Erro ao carregar usuários
                </h3>
                <p className="text-sm text-muted-foreground">
                  Tente novamente mais tarde
                </p>
              </div>
            </div>
          ) : (
            <>
              <div className="rounded-md border mx-6 mb-4">
                <DataTable columns={columns} data={filteredUsers} />
              </div>

              {result && (
                <TablePagination
                  currentPage={result.currentPage}
                  totalPages={result.totalPages}
                  totalCount={result.totalItems}
                  pageSize={pageSize}
                  onPageChange={handlePageChange}
                  onPageSizeChange={handlePageSizeChange}
                  isLoading={isFetching}
                />
              )}
            </>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
