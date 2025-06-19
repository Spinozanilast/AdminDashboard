"use client"

import { useNavigate } from "react-router"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { z } from "zod"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { useLogin } from "@/services/api"
import { ThemeToggle } from "@/components/theme-toggle"
import { Loader2 } from "lucide-react"
import { toast } from "sonner"

const loginSchema = z.object({
    email: z.string().email("Invalid email address"),
    password: z.string().min(1, "Password is required"),
})

type LoginFormValues = z.infer<typeof loginSchema>

export default function LoginPage() {
    const navigate = useNavigate()
    const loginMutation = useLogin()

    const form = useForm<LoginFormValues>({
        resolver: zodResolver(loginSchema),
        defaultValues: {
            email: "admin@mirra.dev",
            password: "admin123",
        },
    })

    const onSubmit = async (data: LoginFormValues) => {
        try {
            await loginMutation.mutateAsync(data)
            toast.success("Welcome to the admin dashboard!")
            navigate("/dashboard")
        } catch (error) {
            toast.error(error instanceof Error ? error.message : "Invalid credentials")
        }
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-background p-4">
            <ThemeToggle />

            <Card className="w-full max-w-md">
                <CardHeader className="space-y-1">
                    <CardTitle className="text-2xl font-bold text-center">Admin Dashboard</CardTitle>
                    <CardDescription className="text-center">Enter your credentials to access the dashboard</CardDescription>
                </CardHeader>
                <CardContent>
                    <Form {...form}>
                        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
                            <FormField
                                control={form.control}
                                name="email"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Email</FormLabel>
                                        <FormControl>
                                            <Input placeholder="admin@mirra.dev" {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="password"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Password</FormLabel>
                                        <FormControl>
                                            <Input type="password" placeholder="••••••••" {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <Button type="submit" className="w-full" disabled={loginMutation.isPending}>
                                {loginMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                Sign In
                            </Button>
                        </form>
                    </Form>

                    <div className="mt-4 text-center text-sm text-muted-foreground">
                        <p>Demo credentials:</p>
                        <p>Email: admin@mirra.dev</p>
                        <p>Password: admin123</p>
                    </div>
                </CardContent>
            </Card>
        </div>
    )
}
