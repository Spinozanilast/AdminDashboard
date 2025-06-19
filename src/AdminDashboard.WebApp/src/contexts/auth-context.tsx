"use client"

import { createContext, useContext, useState, useEffect, type ReactNode } from "react"

interface AuthContextType {
    token: string | null
    refreshToken: string | null
    isAuthenticated: boolean
    login: (token: string, refreshToken: string) => void
    logout: () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
    const [token, setToken] = useState<string | null>(localStorage.getItem("token"))
    const [refreshToken, setRefreshToken] = useState<string | null>(localStorage.getItem("refreshToken"))
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(!!token)

    useEffect(() => {
        if (token) {
            localStorage.setItem("token", token)
            setIsAuthenticated(true)
        } else {
            localStorage.removeItem("token")
            setIsAuthenticated(false)
        }
    }, [token])

    useEffect(() => {
        if (refreshToken) {
            localStorage.setItem("refreshToken", refreshToken)
        } else {
            localStorage.removeItem("refreshToken")
        }
    }, [refreshToken])

    const login = (newToken: string, newRefreshToken: string) => {
        setToken(newToken)
        setRefreshToken(newRefreshToken)
    }

    const logout = () => {
        setToken(null)
        setRefreshToken(null)
    }

    return (
        <AuthContext.Provider value={{ token, refreshToken, isAuthenticated, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth() {
    const context = useContext(AuthContext)
    if (context === undefined) {
        throw new Error("useAuth must be used within an AuthProvider")
    }
    return context
}
