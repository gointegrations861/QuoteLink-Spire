﻿Namespace My
    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_UnhandledException(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show(e.Exception.Message & Environment.NewLine & Environment.NewLine & "Please contact FCAS, Inc. to resolve this issue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            My.Application.Log.WriteException(e.Exception,
                TraceEventType.Critical,
                "Application shut down at " &
                My.Computer.Clock.LocalTime.ToString & Environment.NewLine &
                e.Exception.StackTrace)
        End Sub
    End Class


End Namespace

