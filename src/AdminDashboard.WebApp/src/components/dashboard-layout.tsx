"use client"

import type { ReactNode } from "react"
import { Button } from "@/components/ui/button"
import { ThemeToggle } from "@/components/theme-toggle"

interface DashboardLayoutProps {
    children: ReactNode
    onLogout: () => void
}

export default function DashboardLayout({ children, onLogout }: DashboardLayoutProps) {
    return (
        <div className="min-h-screen bg-background">
            <header className="sticky top-0 z-10 border-b bg-background">
                <div className="container flex h-16 items-center justify-between">
                    <h1 className="text-xl font-bold">Admin Dashboard</h1>
                    <div className="flex items-center gap-4">
                        <ThemeToggle />
                        <Button variant="outline" onClick={onLogout}>
                            Logout
                        </Button>
                    </div>
                </div>
            </header>
            <main className="container py-6">{children}</main>
        </div>
    )
}
