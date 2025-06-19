"use client"

import { useEffect, useState } from "react"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { z } from "zod"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Badge } from "@/components/ui/badge"
import { Skeleton } from "@/components/ui/skeleton"
import { Checkbox } from "@/components/ui/checkbox"
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { Plus, Edit, Trash2, Users } from "lucide-react"
import { ApiError, useClients, useCreateClient, useDeleteClients, useUpdateClient, type Client } from "@/services/api"
import { toast } from "sonner"

const clientSchema = z.object({
    name: z.string().min(1, "Name is required"),
    email: z.string().email("Invalid email address"),
    balanceT: z.number().min(0, "Balance must be non-negative"),
    tags: z.array(z.string()),
})

type ClientFormValues = z.infer<typeof clientSchema>

// Update the component to use enhanced hooks and better error handling
export function ClientsManagement() {
    const [selectedClients, setSelectedClients] = useState<string[]>([])
    const [editingClient, setEditingClient] = useState<Client | null>(null)
    const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false)
    const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)

    const clientsQuery = useClients()
    const createClientMutation = useCreateClient()
    const updateClientMutation = useUpdateClient()
    const deleteClientsMutation = useDeleteClients()

    const createForm = useForm<ClientFormValues>({
        resolver: zodResolver(clientSchema),
        defaultValues: {
            name: "",
            email: "",
            balanceT: 0,
            tags: [],
        },
    })

    const editForm = useForm<ClientFormValues>({
        resolver: zodResolver(clientSchema),
        defaultValues: {
            name: "",
            email: "",
            balanceT: 0,
            tags: [],
        },
    })

    useEffect(() => {
        if (!isCreateDialogOpen) {
            createForm.reset();
        }
    }, [isCreateDialogOpen, createForm]);

    useEffect(() => {
        if (!isEditDialogOpen) {
            editForm.reset();
            setEditingClient(null);
        }
    }, [isEditDialogOpen, editForm]);

    const handleApiError = (error: unknown, defaultMessage: string) => {
        if (error instanceof ApiError) {
            if (error.validationErrors) {
                // Handle validation errors
                const validationMessages = Object.entries(error.validationErrors)
                    .map(([field, messages]) => `${field}: ${messages.join(", ")}`)
                    .join("\n")

                toast.error(validationMessages)
                return
            }

            if (error.problemDetails) {
                toast.error(error.problemDetails.detail)
                return
            }

            toast.error(error.message)
            return
        }

        toast.error(defaultMessage)
    }

    const onCreateSubmit = async (data: ClientFormValues) => {
        try {
            await createClientMutation.mutateAsync(data)
            toast.success(`${data.name} has been created successfully.`)
            setIsCreateDialogOpen(false)
            createForm.reset()
        } catch (error) {
            handleApiError(error, "Failed to create client")
            setIsCreateDialogOpen(false)
        }
    }

    const onEditSubmit = async (data: ClientFormValues) => {
        if (!editingClient) return

        try {
            await updateClientMutation.mutateAsync({
                id: editingClient.id,
                data: { ...data, id: editingClient.id, tags: editingClient.tags },
            })
            toast.info(`${data.name} has been updated successfully.`)
            setIsEditDialogOpen(false)
            setEditingClient(null)
            editForm.reset()
        } catch (error) {
            handleApiError(error, "Failed to update client")
            setIsCreateDialogOpen(false)
        }
    }

    const handleDeleteSelected = async () => {
        if (selectedClients.length === 0) return

        try {
            await deleteClientsMutation.mutateAsync({ clientIds: selectedClients })
            toast.warning(`${selectedClients.length} client(s) have been deleted.`)
            setSelectedClients([])
        } catch (error) {
            handleApiError(error, "Failed to delete clients")
        }
    }

    const handleEdit = (client: Client) => {
        setEditingClient(client)
        editForm.reset({
            name: client.name,
            email: client.email,
            balanceT: client.balanceT,
        })
        setIsEditDialogOpen(true)
    }

    const handleSelectAll = (checked: boolean) => {
        if (checked && clientsQuery.data) {
            setSelectedClients(clientsQuery.data.map((client) => client.id))
        } else {
            setSelectedClients([])
        }
    }

    const handleSelectClient = (clientId: string, checked: boolean) => {
        if (checked) {
            setSelectedClients((prev) => [...prev, clientId])
        } else {
            setSelectedClients((prev) => prev.filter((id) => id !== clientId))
        }
    }

    const handleDeleteSingle = async (clientId: string) => {
        try {
            await deleteClientsMutation.mutateAsync({ clientIds: [clientId] })
            toast.warning("Client has been deleted successfully.")
        } catch (error) {
            handleApiError(error, "Failed to delete client")
        }
    }

    if (clientsQuery.isLoading) {
        return (
            <Card>
                <CardHeader>
                    <CardTitle className="flex items-center gap-2">
                        <Users className="h-5 w-5" />
                        Clients
                    </CardTitle>
                </CardHeader>
                <CardContent>
                    <div className="space-y-3">
                        {Array.from({ length: 5 }).map((_, i) => (
                            <div key={i} className="flex items-center space-x-4">
                                <Skeleton className="h-4 w-4" />
                                <Skeleton className="h-12 w-12 rounded-full" />
                                <div className="space-y-2">
                                    <Skeleton className="h-4 w-[200px]" />
                                    <Skeleton className="h-4 w-[160px]" />
                                </div>
                            </div>
                        ))}
                    </div>
                </CardContent>
            </Card>
        )
    }

    if (clientsQuery.error) {
        return (
            <Card>
                <CardHeader>
                    <CardTitle className="flex items-center gap-2">
                        <Users className="h-5 w-5" />
                        Clients
                    </CardTitle>
                </CardHeader>
                <CardContent>
                    <div className="text-center py-8 text-destructive">Error loading clients: {clientsQuery.error.message}</div>
                </CardContent>
            </Card>
        )
    }

    const clients = clientsQuery.data || []

    return (
        <Card>
            <CardHeader>
                <CardTitle className="flex items-center gap-2">
                    <Users className="h-5 w-5" />
                    Clients ({clients.length})
                </CardTitle>
                <CardDescription>Manage your client database</CardDescription>
            </CardHeader>
            <CardContent>
                <div className="flex justify-between items-center mb-4">
                    <div className="flex items-center gap-2">
                        {selectedClients.length > 0 && (
                            <AlertDialog>
                                <AlertDialogTrigger asChild>
                                    <Button variant="destructive" size="sm">
                                        <Trash2 className="h-4 w-4 mr-2" />
                                        Delete Selected ({selectedClients.length})
                                    </Button>
                                </AlertDialogTrigger>
                                <AlertDialogContent>
                                    <AlertDialogHeader>
                                        <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                                        <AlertDialogDescription>
                                            This action cannot be undone. This will permanently delete {selectedClients.length} client(s).
                                        </AlertDialogDescription>
                                    </AlertDialogHeader>
                                    <AlertDialogFooter>
                                        <AlertDialogCancel asChild>Cancel</AlertDialogCancel>
                                        <AlertDialogAction
                                            onClick={() => handleDeleteSelected()}
                                            disabled={deleteClientsMutation.isPending}
                                            className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
                                        >
                                            {deleteClientsMutation.isPending ? "Deleting..." : "Delete"}
                                        </AlertDialogAction>
                                    </AlertDialogFooter>
                                </AlertDialogContent>
                            </AlertDialog>
                        )}
                    </div>

                    <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
                        <DialogTrigger asChild>
                            <Button>
                                <Plus className="h-4 w-4 mr-2" />
                                Add Client
                            </Button>
                        </DialogTrigger>
                        <DialogContent showCloseButton={false}>
                            <DialogHeader>
                                <DialogTitle>Create New Client</DialogTitle>
                                <DialogDescription>Add a new client to your database.</DialogDescription>
                            </DialogHeader>
                            <Form {...createForm}>
                                <form onSubmit={createForm.handleSubmit(onCreateSubmit)} className="space-y-4">
                                    <FormField
                                        control={createForm.control}
                                        name="name"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Name</FormLabel>
                                                <FormControl>
                                                    <Input placeholder="Enter client name" {...field} />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                    <FormField
                                        control={createForm.control}
                                        name="email"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Email</FormLabel>
                                                <FormControl>
                                                    <Input type="email" placeholder="client@example.com" {...field} />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                    <FormField
                                        control={createForm.control}
                                        name="balanceT"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Balance T</FormLabel>
                                                <FormControl>
                                                    <Input
                                                        type="number"
                                                        step="0.01"
                                                        min="0"
                                                        placeholder="0.00"
                                                        {...field}
                                                        onChange={(e) => field.onChange(Number.parseFloat(e.target.value) || 0)}
                                                    />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                    <DialogFooter>
                                        <DialogClose asChild>
                                            <Button type="button" variant="outline">
                                                Cancel
                                            </Button>
                                        </DialogClose>
                                        <Button type="submit" disabled={createClientMutation.isPending}>
                                            {createClientMutation.isPending ? "Creating..." : "Create Client"}
                                        </Button>
                                    </DialogFooter>
                                </form>
                            </Form>
                        </DialogContent>
                    </Dialog>
                </div>

                <div className="border rounded-lg">
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead className="w-12">
                                    <Checkbox
                                        checked={clients.length > 0 && selectedClients.length === clients.length}
                                        onCheckedChange={handleSelectAll}
                                        aria-label="Select all clients"
                                    />
                                </TableHead>
                                <TableHead>Name</TableHead>
                                <TableHead>Email</TableHead>
                                <TableHead className="text-right">Balance T</TableHead>
                                <TableHead className="text-center">Tags</TableHead>
                                <TableHead className="text-right">Actions</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {clients.length === 0 ? (
                                <TableRow>
                                    <TableCell colSpan={5} className="text-center py-8 text-muted-foreground">
                                        No clients found. Create your first client to get started.
                                    </TableCell>
                                </TableRow>
                            ) : (
                                clients.map((client) => (
                                    <TableRow key={client.id}>
                                        <TableCell>
                                            <Checkbox
                                                checked={selectedClients.includes(client.id)}
                                                onCheckedChange={(checked) => handleSelectClient(client.id, checked as boolean)}
                                                aria-label={`Select ${client.name}`}
                                            />
                                        </TableCell>
                                        <TableCell className="font-medium">{client.name}</TableCell>
                                        <TableCell className="text-muted-foreground">{client.email}</TableCell>
                                        <TableCell className="text-right">
                                            <Badge variant="secondary">{client.balanceT.toFixed(2)}</Badge>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex flex-wrap justify-center gap-2">
                                                {client.tags.map((tag) => {
                                                    return (
                                                        <Badge key={tag.name} style={
                                                            { backgroundColor: tag.color }
                                                        } >
                                                            {tag.name}
                                                        </Badge>)
                                                }
                                                )}
                                            </div>
                                        </TableCell>
                                        <TableCell className="text-right">
                                            <div className="flex justify-end gap-2">
                                                <Button variant="outline" size="sm" onClick={() => handleEdit(client)}>
                                                    <Edit className="h-4 w-4" />
                                                </Button>
                                                <AlertDialog key={`delete-dialog-${client.id}`}>
                                                    <AlertDialogTrigger asChild>
                                                        <Button variant="outline" size="sm">
                                                            <Trash2 className="h-4 w-4" />
                                                        </Button>
                                                    </AlertDialogTrigger>
                                                    <AlertDialogContent>
                                                        <AlertDialogHeader>
                                                            <AlertDialogTitle>Delete Client</AlertDialogTitle>
                                                            <AlertDialogDescription>
                                                                Are you sure you want to delete {client.name}? This action cannot be undone.
                                                            </AlertDialogDescription>
                                                        </AlertDialogHeader>
                                                        <AlertDialogFooter>
                                                            <AlertDialogCancel>Cancel</AlertDialogCancel>
                                                            <AlertDialogAction
                                                                onClick={() => handleDeleteSingle(client.id)}
                                                                className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
                                                            >
                                                                Delete
                                                            </AlertDialogAction>
                                                        </AlertDialogFooter>
                                                    </AlertDialogContent>
                                                </AlertDialog>
                                            </div>
                                        </TableCell>
                                    </TableRow>
                                ))
                            )}
                        </TableBody>
                    </Table>
                </div>

                {/* Edit Dialog */}
                <Dialog key={`edit-dialog-${editingClient?.id || 'new'}`} open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
                    <DialogContent>
                        <DialogHeader>
                            <DialogTitle>Edit Client</DialogTitle>
                            <DialogDescription>Update client information.</DialogDescription>
                        </DialogHeader>
                        <Form {...editForm}>
                            <form onSubmit={editForm.handleSubmit(onEditSubmit)} className="space-y-4">
                                <FormField
                                    control={editForm.control}
                                    name="name"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Name</FormLabel>
                                            <FormControl>
                                                <Input placeholder="Enter client name" {...field} />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={editForm.control}
                                    name="email"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Email</FormLabel>
                                            <FormControl>
                                                <Input type="email" placeholder="client@example.com" {...field} />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={editForm.control}
                                    name="balanceT"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel>Balance T</FormLabel>
                                            <FormControl>
                                                <Input
                                                    type="number"
                                                    step="0.01"
                                                    min="0"
                                                    placeholder="0.00"
                                                    {...field}
                                                    onChange={(e) => field.onChange(Number.parseFloat(e.target.value) || 0)}
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <DialogFooter>
                                    <Button
                                        type="button"
                                        variant="outline"
                                        onClick={() => {
                                            setIsEditDialogOpen(false)
                                            setEditingClient(null)
                                        }}
                                    >
                                        Cancel
                                    </Button>
                                    <Button type="submit" disabled={updateClientMutation.isPending}>
                                        {updateClientMutation.isPending ? "Updating..." : "Update Client"}
                                    </Button>
                                </DialogFooter>
                            </form>
                        </Form>
                    </DialogContent>
                </Dialog>
            </CardContent>
        </Card>
    )
}
