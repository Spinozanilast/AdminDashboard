"use client"

import { Alert, AlertDescription } from "@/components/ui/alert"
import type { ValidationError } from "@/services/api"
import { AlertTriangle } from "lucide-react"

interface ValidationErrorsProps {
    errors: ValidationError
    className?: string
}

export function ValidationErrors({ errors, className }: ValidationErrorsProps) {
    const errorEntries = Object.entries(errors)

    if (errorEntries.length === 0) {
        return null
    }

    return (
        <Alert variant="destructive" className={className}>
            <AlertTriangle className="h-4 w-4" />
            <AlertDescription>
                <div className="space-y-1">
                    {errorEntries.map(([field, messages]) => (
                        <div key={field}>
                            <strong className="capitalize">{field}:</strong>{" "}
                            {Array.isArray(messages) ? messages.join(", ") : messages}
                        </div>
                    ))}
                </div>
            </AlertDescription>
        </Alert>
    )
}
