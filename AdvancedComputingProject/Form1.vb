Imports System.Data.OleDb
Public Class Form1

    Public Structure playerDetails
        Dim wins As Integer
        Dim playerTag As String
        Dim playerName As String
    End Structure

    Dim myForm2 As New mainPlayspace
    Public players(50) As playerDetails

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        myForm2.Show()
        Me.Hide()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim counter As Integer

        'Connect to database
        Dim SQLReader As OleDbDataReader
        Dim connection_type As String = "Provider=Microsoft.ACE.OLEDB.12.0;"
        Dim file_location As String = "Data Source=N:\Advanced Computing\AH1\test.accdb"
        Dim conn As OleDbConnection
        conn = New OleDbConnection(connection_type & file_location)
        conn.Open()

        'Select and display results
        Dim query As String = "Select * FROM [Players]"
        Dim command As New OleDbCommand(query, conn)
        SQLReader = command.ExecuteReader()
        If SQLReader.HasRows Then
            While SQLReader.Read
                players(counter).wins = SQLReader("Wins")
                players(counter).playerName = SQLReader("PlayerName")
                players(counter).playerTag = SQLReader("playerTag")
                counter = counter + 1
            End While
        Else
            MsgBox("this file isnt valid")
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim counter As Integer

        ListBox1.Items.Clear()

        Do
            ListBox1.Items.Add(players(counter).wins & "  " & players(counter).playerTag & "  " & players(counter).playerName)
            counter = counter + 1
        Loop Until players(counter).wins = 0 And players(counter).playerTag = ""

    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim n As Integer
        Dim counter As Integer
        Dim endarray As Boolean
        Dim player As playerDetails

        endarray = False

        Do
            If players(counter).playerTag = "" Then
                endarray = True
            End If
            counter = counter + 1
        Loop Until counter = 50 Or endarray = True
        n = counter
     
        Dim swapped As Boolean
        swapped = True

        While swapped And n >= 0
            swapped = False
            For counter = 0 To n - 2

                If players(counter).wins < players(counter + 1).wins Then
                    player = players(counter)
                    players(counter) = players(counter + 1)
                    players(counter + 1) = player
                    swapped = True
                End If

            Next counter
            n = n - 1
        End While
    End Sub

End Class
