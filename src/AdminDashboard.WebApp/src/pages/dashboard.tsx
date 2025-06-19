"use client"

import { useState } from "react"
import { useNavigate } from "react-router"
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
import { useClients, useExchangeRate, useUpdateRate, usePayments } from "@/services/api"
import { ThemeToggle } from "@/components/theme-toggle"
import { LogOut, Users, TrendingUp, CreditCard, RefreshCw } from "lucide-react"
import { toast } from "sonner"
import { useAuth } from "@/contexts/auth-context"

const rateSchema = z.object({
    newRate: z.number().min(0.01, "Rate must be greater than 0"),
})

type RateFormValues = z.infer<typeof rateSchema>

export default function DashboardPage() {
    const navigate = useNavigate()
    const { logout } = useAuth()

    // Queries
    const clientsQuery = useClients()
    const rateQuery = useExchangeRate()
    const paymentsQuery = usePayments(5)
    const updateRateMutation = useUpdateRate()

    const form = useForm<RateFormValues>({
        resolver: zodResolver(rateSchema),
        defaultValues: {
            newRate: rateQuery.data?.rate || 10,
        },
    })

    useState(() => {
        if (rateQuery.data?.rate) {
            form.setValue("newRate", rateQuery.data.rate)
        }
    })

    const handleLogout = () => {
        logout()
        navigate("/login")
    }

    const onSubmitRate = async (data: RateFormValues) => {
        try {
            await updateRateMutation.mutateAsync(data)
            toast.info(`Exchange rate updated to ${data.newRate}`)
        } catch (error) {
            toast.error(error instanceof Error ? error.message : "Failed to update rate")
        }
    }

    const refreshData = () => {
        clientsQuery.refetch()
        rateQuery.refetch()
        paymentsQuery.refetch()
    }

    return (
        <div className="min-h-screen bg-background">
            {/* Header */}
            <header className="sticky top-0 z-50 w-full border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
                <div className="container flex h-16 items-center justify-between">
                    <div className="flex items-center gap-2">
                        <div className="h-8 w-8 rounded-lg bg-primary flex items-center justify-center">
                            <TrendingUp className="h-4 w-4 text-primary-foreground" />
                        </div>
                        <h1 className="text-xl font-bold">Admin Dashboard</h1>
                    </div>

                    <div className="flex items-center gap-4">
                        <Button variant="outline" size="icon" onClick={refreshData}>
                            <RefreshCw className="h-4 w-4" />
                        </Button>
                        <ThemeToggle />
                        <Button variant="outline" onClick={handleLogout}>
                            <LogOut className="mr-2 h-4 w-4" />
                            Logout
                        </Button>
                    </div>
                </div>
            </header>

            <main className="container py-8">
                <div className="space-y-8">
                    {/* Stats Cards */}
                    <div className="grid gap-4 md:grid-cols-3">
                        <Card>
                            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                                <CardTitle className="text-sm font-medium">Total Clients</CardTitle>
                                <Users className="h-4 w-4 text-muted-foreground" />
                            </CardHeader>
                            <CardContent>
                                <div className="text-2xl font-bold">
                                    {clientsQuery.isLoading ? <Skeleton className="h-8 w-8" /> : clientsQuery.data?.length || 0}
                                </div>
                            </CardContent>
                        </Card>

                        <Card>
                            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                                <CardTitle className="text-sm font-medium">Exchange Rate</CardTitle>
                                <TrendingUp className="h-4 w-4 text-muted-foreground" />
                            </CardHeader>
                            <CardContent>
                                <div className="text-2xl font-bold">
                                    {rateQuery.isLoading ? <Skeleton className="h-8 w-16" /> : rateQuery.data?.rate.toFixed(2) || "0.00"}
                                </div>
                            </CardContent>
                        </Card>

                        <Card>
                            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                                <CardTitle className="text-sm font-medium">Recent Payments</CardTitle>
                                <CreditCard className="h-4 w-4 text-muted-foreground" />
                            </CardHeader>
                            <CardContent>
                                <div className="text-2xl font-bold">
                                    {paymentsQuery.isLoading ? <Skeleton className="h-8 w-8" /> : paymentsQuery.data?.length || 0}
                                </div>
                            </CardContent>
                        </Card>
                    </div>

                    <div className="grid gap-6 md:grid-cols-2">
                        {/* Clients Table */}
                        <Card>
                            <CardHeader>
                                <CardTitle>Clients</CardTitle>
                                <CardDescription>Manage your client database</CardDescription>
                            </CardHeader>
                            <CardContent>
                                {clientsQuery.isLoading ? (
                                    <div className="space-y-3">
                                        {Array.from({ length: 3 }).map((_, i) => (
                                            <div key={i} className="flex items-center space-x-4">
                                                <Skeleton className="h-12 w-12 rounded-full" />
                                                <div className="space-y-2">
                                                    <Skeleton className="h-4 w-[200px]" />
                                                    <Skeleton className="h-4 w-[160px]" />
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                ) : clientsQuery.error ? (
                                    <div className="text-center py-8 text-destructive">
                                        Error loading clients: {clientsQuery.error.message}
                                    </div>
                                ) : (
                                    <Table>
                                        <TableHeader>
                                            <TableRow>
                                                <TableHead>Name</TableHead>
                                                <TableHead>Email</TableHead>
                                                <TableHead className="text-right">Balance T</TableHead>
                                            </TableRow>
                                        </TableHeader>
                                        <TableBody>
                                            {clientsQuery.data?.length === 0 ? (
                                                <TableRow>
                                                    <TableCell colSpan={3} className="text-center py-8 text-muted-foreground">
                                                        No clients found
                                                    </TableCell>
                                                </TableRow>
                                            ) : (
                                                clientsQuery.data?.map((client) => (
                                                    <TableRow key={client.id}>
                                                        <TableCell className="font-medium">{client.name}</TableCell>
                                                        <TableCell className="text-muted-foreground">{client.email}</TableCell>
                                                        <TableCell className="text-right">
                                                            <Badge variant="secondary">{client.balanceT.toFixed(2)}</Badge>
                                                        </TableCell>
                                                    </TableRow>
                                                ))
                                            )}
                                        </TableBody>
                                    </Table>
                                )}
                            </CardContent>
                        </Card>

                        {/* Exchange Rate */}
                        <Card>
                            <CardHeader>
                                <CardTitle>Exchange Rate</CardTitle>
                                <CardDescription>Manage token exchange rate</CardDescription>
                            </CardHeader>
                            <CardContent>
                                {rateQuery.isLoading ? (
                                    <div className="space-y-4">
                                        <Skeleton className="h-8 w-24" />
                                        <Skeleton className="h-4 w-32" />
                                        <Skeleton className="h-10 w-full" />
                                        <Skeleton className="h-10 w-full" />
                                    </div>
                                ) : rateQuery.error ? (
                                    <div className="text-center py-8 text-destructive">Error loading rate: {rateQuery.error.message}</div>
                                ) : (
                                    <div className="space-y-6">
                                        <div className="text-center">
                                            <div className="text-3xl font-bold text-primary">{rateQuery.data?.rate.toFixed(2)}</div>
                                            <p className="text-sm text-muted-foreground mt-1">
                                                Last updated:{" "}
                                                {rateQuery.data?.lastUpdated
                                                    ? new Date(rateQuery.data.lastUpdated).toLocaleString()
                                                    : "Unknown"}
                                            </p>
                                        </div>

                                        <Form {...form}>
                                            <form onSubmit={form.handleSubmit(onSubmitRate)} className="space-y-4">
                                                <FormField
                                                    control={form.control}
                                                    name="newRate"
                                                    render={({ field }) => (
                                                        <FormItem>
                                                            <FormLabel>New Rate</FormLabel>
                                                            <FormControl>
                                                                <Input
                                                                    type="number"
                                                                    step="0.01"
                                                                    min="0.01"
                                                                    placeholder="Enter new rate"
                                                                    {...field}
                                                                    onChange={(e) => field.onChange(Number.parseFloat(e.target.value))}
                                                                />
                                                            </FormControl>
                                                            <FormMessage />
                                                        </FormItem>
                                                    )}
                                                />
                                                <Button type="submit" className="w-full" disabled={updateRateMutation.isPending}>
                                                    {updateRateMutation.isPending ? "Updating..." : "Update Rate"}
                                                </Button>
                                            </form>
                                        </Form>
                                    </div>
                                )}
                            </CardContent>
                        </Card>
                    </div>

                    {/* Recent Payments */}
                    <Card>
                        <CardHeader>
                            <CardTitle>Recent Payments</CardTitle>
                            <CardDescription>Latest payment transactions</CardDescription>
                        </CardHeader>
                        <CardContent>
                            {paymentsQuery.isLoading ? (
                                <div className="space-y-3">
                                    {Array.from({ length: 5 }).map((_, i) => (
                                        <Skeleton key={i} className="h-12 w-full" />
                                    ))}
                                </div>
                            ) : paymentsQuery.error ? (
                                <div className="text-center py-8 text-destructive">
                                    Error loading payments: {paymentsQuery.error.message}
                                </div>
                            ) : (
                                <Table>
                                    <TableHeader>
                                        <TableRow>
                                            <TableHead>Client</TableHead>
                                            <TableHead>Amount</TableHead>
                                            <TableHead>Tokens</TableHead>
                                            <TableHead>Status</TableHead>
                                            <TableHead>Date</TableHead>
                                        </TableRow>
                                    </TableHeader>
                                    <TableBody>
                                        {paymentsQuery.data?.length === 0 ? (
                                            <TableRow>
                                                <TableCell colSpan={5} className="text-center py-8 text-muted-foreground">
                                                    No payments found
                                                </TableCell>
                                            </TableRow>
                                        ) : (
                                            paymentsQuery.data?.map((payment) => (
                                                <TableRow key={payment.id}>
                                                    <TableCell className="font-medium">{payment.clientName}</TableCell>
                                                    <TableCell>${payment.amount.toFixed(2)}</TableCell>
                                                    <TableCell>{payment.tokensAmount.toFixed(2)}</TableCell>
                                                    <TableCell>
                                                        <Badge variant={payment.status === "completed" ? "default" : "secondary"}>
                                                            {payment.status}
                                                        </Badge>
                                                    </TableCell>
                                                    <TableCell>{new Date(payment.createdAt).toLocaleDateString()}</TableCell>
                                                </TableRow>
                                            ))
                                        )}
                                    </TableBody>
                                </Table>
                            )}
                        </CardContent>
                    </Card>
                </div>
            </main>
        </div>
    )
}
