Imports System.Media
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Runtime.InteropServices
Imports System.Data.OleDb

Public Class mainPlayspace

    Public Structure cardDetails
        Dim Type As String
        Dim Colour As String
        Dim Addition As String
        Dim Number As String
    End Structure

    Public Structure playerDetails
        Dim playerName As String
        Dim playerTag As String
        Dim Wins As Integer
    End Structure

    '------------------------------------------------------------------------------------------
    'Variables

    'Declaring variables
    Dim player As playerDetails
    Dim gameFin As Boolean
    Dim NPC1 As String
    Dim NPC2 As String
    Dim NPC3 As String
    Dim NPC1Count As String
    Dim NPC2Count As String
    Dim NPC3Count As String
    Dim deck(7) As cardDetails
    Dim centreDeck(55) As cardDetails
    Dim playedCard As cardDetails
    Dim playerDeck(7) As cardDetails
    Dim NPC1Deck(7) As cardDetails
    Dim NPC2Deck(7) As cardDetails
    Dim NPC3Deck(7) As cardDetails
    Dim filename As String = "N:\Advanced Computing\cards finale\"
    Dim played As Boolean = False
    Dim visible As Boolean
    Dim playCardCard As cardDetails
    Dim adding As Integer
    Dim deckCounter As Integer
    Dim takenCard As cardDetails
    Dim playerDeckCount As Integer
    Dim players(100) As playerDetails
    Dim placeholderAIDeck() As cardDetails

    '------------------------------------------------------------------------------------------
    'Procedure to execute on load
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        NPC1Count = NPC1Deck.Count
        NPC2Count = NPC2Deck.Count
        NPC3Count = NPC3Deck.Count

        Dim rName As String = ""

        player.playerName = InputBox("Please enter your name")
        player.playerTag = InputBox("Please enter your player tag")

        validateTag(player)

        player_label.Text = player.playerTag
        txtConsole.Text += "Name Generation" & vbNewLine

        'Setting NPC names
        randomiseName(rName)
        NPC1 = rName
        npc1_label.Text = "" & NPC1

        randomiseName(rName)
        NPC2 = rName
        npc2_label.Text = "" & NPC2

        randomiseName(rName)
        NPC3 = rName
        npc3_label.Text = "" & NPC3

        'Setting up UI
        Call populateDeck(centreDeck)
        Call dealCards(centreDeck, playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard, deckCounter)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        NPCdeckCount1.Text = NPC1Count
        NPCdeckCount2.Text = NPC2Count
        NPCdeckCount3.Text = NPC3Count

    End Sub

    '------------------------------------------------------------------------------------------
    'Function to validate inputs for playerTag
    Function validateTag(ByRef player As playerDetails)

        Do
            If Len(player.playerTag) <> 4 Then
                player.playerTag = InputBox("Please enter a four letter tag")
            End If

        Loop Until Len(player.playerTag) = 4

        player.playerTag = UCase(player.playerTag)

        Return player

    End Function
    'Function to randomise the names of other players
    Function randomiseName(ByRef rName As String)

        rName = ""
        Randomize()

        Dim counter As Integer
        Dim alphabet(26) As String
        Dim letter(4) As String
        Dim rand As Integer

        For counter = 0 To 25

            alphabet(counter) = Chr(65 + counter)

        Next counter

        For counter = 0 To 3

            rand = Int(25 * Rnd())
            letter(counter) = alphabet(rand)

            txtConsole.Text += "" & rand & ", "

        Next counter

        rName = letter(0) & letter(1) & letter(2) & letter(3)
        rName = UCase(rName)

        txtConsole.Text += vbNewLine

        rand = 0

        Return rName
    End Function
    'Function to display player deck
    Function displayCard(ByRef filename As String, ByRef counter As Integer)

        filename = "N:\Advanced Computing\cards finale\"

        If playerDeck(counter).Type = "colour" And playerDeck(counter).Number <> "Null" Then
            filename = filename & playerDeck(counter).Colour & playerDeck(counter).Number & ".png"
        ElseIf playerDeck(counter).Type = "reverse" Then
            filename = filename & playerDeck(counter).Colour & "reverse.png"
        ElseIf playerDeck(counter).Type = "plus" Then
            filename = filename & playerDeck(counter).Colour & "+" & playerDeck(counter).Addition & ".png"
        ElseIf playerDeck(counter).Type = "" Then
            filename = ""
        End If

        Return filename

    End Function
    'Function to display centre deck
    Function displayCentreDeck(ByRef filename As String)

        filename = "N:\Advanced Computing\cards finale\"

        If playedCard.Type = "colour" And playedCard.Number <> "Null" Then
            filename = filename & playedCard.Colour & playedCard.Number & ".png"
        ElseIf playedCard.Type = "reverse" Then
            filename = filename & playedCard.Colour & "reverse.png"
        ElseIf playedCard.Type = "plus" Then
            filename = filename & playedCard.Colour & "+" & playedCard.Addition & ".png"
        End If

        Return filename

    End Function


    '------------------------------------------------------------------------------------------
    'Game functions
    Function playCard(ByRef playCardCard As cardDetails, ByRef playedCard As cardDetails, ByRef counter As Integer, ByRef played As Boolean)

        played = False


        If (playedCard.Colour = playCardCard.Colour And playedCard.Colour <> "null" And playCardCard.Addition = "null") Or (playCardCard.Number = playedCard.Number And playCardCard.Addition = "null") Then

            playedCard = playCardCard
            playCardCard.Addition = ""
            playCardCard.Type = ""
            playCardCard.Colour = ""
            playCardCard.Number = ""

            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""

            played = True


        ElseIf playedCard.Colour = playCardCard.Colour And playCardCard.Addition = "2" Or playCardCard.Addition = "4" Then

            playedCard = playCardCard
            playCardCard.Addition = ""
            playCardCard.Type = ""
            playCardCard.Colour = ""
            playCardCard.Number = ""
            played = True
            adding = adding + playedCard.Addition

            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""

            If adding <> 0 Then
                If adding = 2 Then
                    Call AItakeCard(placeholderAIDeck)
                    Call AItakeCard(placeholderAIDeck)
                    NPC3Count = NPC3Count + 2
                    adding = 0
                ElseIf adding = 4 Then
                    Call AItakeCard(placeholderAIDeck)
                    Call AItakeCard(placeholderAIDeck)
                    Call AItakeCard(placeholderAIDeck)
                    Call AItakeCard(placeholderAIDeck)
                    NPC3Count = NPC3Count + 4
                    adding = 0
                End If

            End If

        Else
            MsgBox("Please play a valid card")

        End If

        Call gamePlay(gameFin)
        Return played

    End Function

    'AI PLAY CARD
    Function AIplayCard(ByRef playCardCard As cardDetails, ByRef playedCard As cardDetails, ByRef counter As Integer, ByRef played As Boolean)

        counter = 0
        played = False

        Do

            playCardCard = deck(counter)
            If playedCard.Colour = playCardCard.Colour And playedCard.Colour <> "null" Or playCardCard.Number = playedCard.Number And playCardCard.Addition = "null" Then

                playedCard = playCardCard
                playCardCard.Addition = ""
                playCardCard.Type = ""
                playCardCard.Colour = ""
                playCardCard.Number = ""

                NPC1Deck(counter).Addition = ""
                NPC1Deck(counter).Type = ""
                NPC1Deck(counter).Colour = ""
                NPC1Deck(counter).Number = ""

                played = True


            ElseIf playedCard.Colour = playCardCard.Colour And (playCardCard.Addition = "2" Or playCardCard.Addition = "4") Then

                playedCard = playCardCard
                playCardCard.Addition = ""
                playCardCard.Type = ""
                playCardCard.Colour = ""
                playCardCard.Number = ""
                played = True
                adding = adding + playedCard.Addition

                NPC1Deck(counter).Addition = ""
                NPC1Deck(counter).Type = ""
                NPC1Deck(counter).Colour = ""
                NPC1Deck(counter).Number = ""

            End If


            counter = counter + 1

        Loop Until played = True Or counter = 8

        If played = False Then
            AItakeCard(placeholderAIDeck)
        End If

        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        Return played

    End Function
    Function AIplayCard2(ByRef playCardCard As cardDetails, ByRef playedCard As cardDetails, ByRef counter As Integer, ByRef played As Boolean)

        counter = 0
        played = False

        Do

            playCardCard = deck(counter)
            If playedCard.Colour = playCardCard.Colour And playedCard.Colour <> "null" Or playCardCard.Number = playedCard.Number And playCardCard.Addition = "null" Then

                playedCard = playCardCard
                playCardCard.Addition = ""
                playCardCard.Type = ""
                playCardCard.Colour = ""
                playCardCard.Number = ""

                NPC2Deck(counter).Addition = ""
                NPC2Deck(counter).Type = ""
                NPC2Deck(counter).Colour = ""
                NPC2Deck(counter).Number = ""

                played = True


            ElseIf playedCard.Colour = playCardCard.Colour And (playCardCard.Addition = "2" Or playCardCard.Addition = "4") Then

                playedCard = playCardCard
                playCardCard.Addition = ""
                playCardCard.Type = ""
                playCardCard.Colour = ""
                playCardCard.Number = ""
                played = True
                adding = adding + playedCard.Addition

                NPC2Deck(counter).Addition = ""
                NPC2Deck(counter).Type = ""
                NPC2Deck(counter).Colour = ""
                NPC2Deck(counter).Number = ""

            End If

            counter = counter + 1

        Loop Until played = True Or counter = 8

        If played = False Then
            AItakeCard2(placeholderAIDeck)
        End If

        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        Return played

    End Function
    Function AIplayCard3(ByRef playCardCard As cardDetails, ByRef playedCard As cardDetails, ByRef counter As Integer, ByRef played As Boolean)

        counter = 0
        played = False

        Do

            playCardCard = deck(counter)
            If playedCard.Colour = playCardCard.Colour And playedCard.Colour <> "null" Or playCardCard.Number = playedCard.Number And playCardCard.Addition = "null" Then

                playedCard = playCardCard
                playCardCard.Addition = ""
                playCardCard.Type = ""
                playCardCard.Colour = ""
                playCardCard.Number = ""

                NPC3Deck(counter).Addition = ""
                NPC3Deck(counter).Type = ""
                NPC3Deck(counter).Colour = ""
                NPC3Deck(counter).Number = ""

                played = True


            ElseIf playedCard.Colour = playCardCard.Colour And (playCardCard.Addition = "2" Or playCardCard.Addition = "4") Then

                playedCard = playCardCard
                playCardCard.Addition = ""
                playCardCard.Type = ""
                playCardCard.Colour = ""
                playCardCard.Number = ""
                played = True
                adding = adding + playedCard.Addition

                NPC3Deck(counter).Addition = ""
                NPC3Deck(counter).Type = ""
                NPC3Deck(counter).Colour = ""
                NPC3Deck(counter).Number = ""

            End If

            counter = counter + 1

        Loop Until played = True Or counter = 8

        If played = False Then
            AItakeCard3(placeholderAIDeck)
        End If

        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        Return played

    End Function

    'AI TAKE
    Private Sub takeCard_Click(sender As Object, e As EventArgs) Handles takeCard.Click

        Dim taken As Boolean
        Dim counter As Integer

        takenCard = centreDeck(deckCounter)

        Do
            If playerDeck(counter).Addition = "" And playerDeck(counter).Type = "" And playerDeck(counter).Colour = "" And playerDeck(counter).Number = "" Then
                playerDeck(counter) = takenCard
                taken = True
                visible = False
                visibility(visible)
                deckCounter = deckCounter - 1
            End If
            counter = counter + 1
        Loop Until deckCounter = 7 Or taken = True


        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        Call gamePlay(gameFin)

    End Sub
    Sub AItakeCard(ByRef placeholderAIDeck() As cardDetails)

        Dim taken As Boolean
        Dim counter As Integer
        counter = 0

        takenCard = centreDeck(deckCounter)

        Do
            If NPC1Deck(counter).Addition = "" And NPC1Deck(counter).Type = "" And NPC1Deck(counter).Colour = "" And NPC1Deck(counter).Number = "" Then
                NPC1Deck(counter) = takenCard
                taken = True
                visible = False
                visibility(visible)
            End If
            counter = counter + 1
        Loop Until counter = 7 Or taken = True

    End Sub
    Sub AItakeCard3(ByRef placeholderAIDeck() As cardDetails)

        Dim taken As Boolean
        Dim counter As Integer
        counter = 0

        takenCard = centreDeck(deckCounter)

        Do
            If NPC3Deck(counter).Addition = "" And NPC3Deck(counter).Type = "" And NPC3Deck(counter).Colour = "" And NPC3Deck(counter).Number = "" Then
                NPC3Deck(counter) = takenCard
                taken = True
                visible = False
                visibility(visible)
            End If
            counter = counter + 1
        Loop Until counter = 7 Or taken = True

    End Sub
    Sub AItakeCard2(ByRef placeholderAIDeck() As cardDetails)

        Dim taken As Boolean
        Dim counter As Integer
        counter = 0

        takenCard = centreDeck(deckCounter)

        Do
            If NPC2Deck(counter).Addition = "" And NPC2Deck(counter).Type = "" And NPC2Deck(counter).Colour = "" And NPC2Deck(counter).Number = "" Then
                NPC2Deck(counter) = takenCard
                taken = True
                visible = False
                visibility(visible)
            End If
            counter = counter + 1
        Loop Until counter = 7 Or taken = True

    End Sub


    '------------------------------------------------------------------------------------------
    'Procedures to execute in load
    Sub populateDeck(ByRef centreDeck() As cardDetails)

        Dim txtfileName As String
        Dim counter As Integer = 0
        Dim t As Integer
        Dim Temp As cardDetails
        txtfileName = "N:\Advanced Computing\uno.txt"

        Randomize()

        'Reading from deck file
        FileOpen(1, txtfileName, OpenMode.Input)

        Do

            Input(1, centreDeck(counter).Type)
            Input(1, centreDeck(counter).Colour)
            Input(1, centreDeck(counter).Number)
            Input(1, centreDeck(counter).Addition)

            txtConsole.Text += centreDeck(counter).Type & ", "
            txtConsole.Text += centreDeck(counter).Colour & ", "
            txtConsole.Text += centreDeck(counter).Number & ", "
            txtConsole.Text += centreDeck(counter).Addition & vbNewLine

            counter = counter + 1

        Loop Until EOF(1)

        FileClose(1)


        For index As Integer = 0 To centreDeck.Count - 1
            Dim rnd As New Random
            t = rnd.Next(0, centreDeck.Count - 1)
            Temp = centreDeck(t)
            centreDeck(t) = centreDeck(index)
            centreDeck(index) = Temp
        Next

        txtConsole.Text += vbNewLine & "Randomised Array" & vbNewLine

        For counter = 0 To 55
            txtConsole.Text += centreDeck(counter).Type & ", "
            txtConsole.Text += centreDeck(counter).Colour & ", "
            txtConsole.Text += centreDeck(counter).Number & ", "
            txtConsole.Text += centreDeck(counter).Addition & vbNewLine
        Next counter

    End Sub
    'Procedures to deal out cards
    Sub dealCards(ByRef centreDeck() As cardDetails,
                  ByRef playerDeck() As cardDetails,
                  ByRef NPC1Deck() As cardDetails,
                  ByRef NPC2Deck() As cardDetails,
                  ByRef NPC3Deck() As cardDetails,
                  ByRef playedCard As cardDetails,
                  ByRef deckcounter As Integer)

        Dim counter As Integer
        Dim counter2 As Integer = 0

        For counter = 0 To 7

            playerDeck(counter) = centreDeck(counter2)
            counter2 = counter2 + 1
            NPC1Deck(counter) = centreDeck(counter2)
            counter2 = counter2 + 1
            NPC2Deck(counter) = centreDeck(counter2)
            counter2 = counter2 + 1
            NPC3Deck(counter) = centreDeck(counter2)
            counter2 = counter2 + 1

        Next counter

        playedCard = centreDeck(32)
        deckcounter = counter2

    End Sub
    'Procedures to display decks
    Sub displayDecks(ByRef playerDeck() As cardDetails,
                     ByRef NPC1Deck() As cardDetails,
                     ByRef NPC2Deck() As cardDetails,
                     ByRef NPC3Deck() As cardDetails,
                     ByRef playedCard As cardDetails)

        Dim counter As Integer = 0

        PlayerDeck1.Text = playerDeck(0).Type & vbNewLine & playerDeck(0).Colour & vbNewLine & "No." & playerDeck(0).Number & vbNewLine & "+" & playerDeck(0).Addition
        PlayerDeck2.Text = playerDeck(1).Type & vbNewLine & playerDeck(1).Colour & vbNewLine & "No." & playerDeck(1).Number & vbNewLine & "+" & playerDeck(1).Addition
        PlayerDeck3.Text = playerDeck(2).Type & vbNewLine & playerDeck(2).Colour & vbNewLine & "No." & playerDeck(2).Number & vbNewLine & "+" & playerDeck(2).Addition
        PlayerDeck4.Text = playerDeck(3).Type & vbNewLine & playerDeck(3).Colour & vbNewLine & "No." & playerDeck(3).Number & vbNewLine & "+" & playerDeck(3).Addition
        PlayerDeck5.Text = playerDeck(4).Type & vbNewLine & playerDeck(4).Colour & vbNewLine & "No." & playerDeck(4).Number & vbNewLine & "+" & playerDeck(4).Addition
        PlayerDeck6.Text = playerDeck(5).Type & vbNewLine & playerDeck(5).Colour & vbNewLine & "No." & playerDeck(5).Number & vbNewLine & "+" & playerDeck(5).Addition
        PlayerDeck7.Text = playerDeck(6).Type & vbNewLine & playerDeck(6).Colour & vbNewLine & "No." & playerDeck(6).Number & vbNewLine & "+" & playerDeck(6).Addition
        PlayerDeck8.Text = playerDeck(7).Type & vbNewLine & playerDeck(7).Colour & vbNewLine & "No." & playerDeck(7).Number & vbNewLine & "+" & playerDeck(7).Addition

        playedCardBox.Text = playedCard.Type & vbNewLine & playedCard.Colour & vbNewLine & "No." & playedCard.Number & vbNewLine & "+" & playedCard.Addition


        displayCard(filename, counter)
        CardDisplay1.ImageLocation = filename
        counter = counter + 1

        displayCard(filename, counter)
        CardDisplay2.ImageLocation = filename
        counter = counter + 1

        displayCard(filename, counter)
        CardDisplay3.ImageLocation = filename
        counter = counter + 1

        displayCard(filename, counter)
        CardDisplay4.ImageLocation = filename
        counter = counter + 1

        displayCard(filename, counter)
        CardDisplay5.ImageLocation = filename
        counter = counter + 1

        displayCard(filename, counter)
        CardDisplay6.ImageLocation = filename
        counter = counter + 1

        displayCard(filename, counter)
        CardDisplay7.ImageLocation = filename
        counter = counter + 1

        displayCard(filename, counter)
        CardDisplay8.ImageLocation = filename
        counter = counter + 1

        displayCentreDeck(filename)
        DisplayCentreCard.ImageLocation = filename


        NPCdeckCount1.Text = NPC1Count
        NPCdeckCount2.Text = NPC2Count
        NPCdeckCount3.Text = NPC3Count

    End Sub
    'Toggle visibilty of buttons
    Sub visibility(ByRef visible As Integer)
        PlayerCard1.Visible = visible
        PlayerCard2.Visible = visible
        PlayerCard3.Visible = visible
        PlayerCard4.Visible = visible
        PlayerCard5.Visible = visible
        PlayerCard6.Visible = visible
        PlayerCard7.Visible = visible
        PlayerCard8.Visible = visible
        takeCard.Visible = visible
    End Sub
    'Toggle visibilty of buttons

    '------------------------------------------------------------------------------------------
    Sub gamePlay(ByRef gameFin As Boolean)

        playerDeckCount = playerDeckCount - 1

        While visible = False And NPC1Count <> 0 And NPC2Count <> 0 And NPC3Count <> 0

            Call AIPLay1()
            If played = True Then
                NPC1Count = NPC1Count - 1
                Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
                losegame()
            End If

            Call AIPLay2()
            If played = True Then
                NPC2Count = NPC2Count - 1
                Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
                losegame()
            End If

            Call AIPLay3()
            If played = True Then
                NPC3Count = NPC3Count - 1
                Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
                losegame()
            End If


            visible = True
            Call visibility(visible)
        End While
    End Sub

    Function losegame()
        If NPC1Count = 0 Or NPC2Count = 0 Or NPC3Count = 0 Then

            If NPC1Count = 0 Then
                MsgBox("You lose")

                'Connect to database
                Dim SQLReader As OleDbDataReader
                Dim connection_type As String = "Provider=Microsoft.ACE.OLEDB.12.0;"
                Dim file_location As String = "Data Source=N:\Advanced Computing\AH1\test.accdb"
                Dim conn As OleDbConnection
                conn = New OleDbConnection(connection_type & file_location)
                conn.Open()

                'Convert stored data to SQL query - edited from book!!!
                Dim query As String = "INSERT INTO [Players] VALUES ( 'Null' , '" & NPC1 & "', '1');"

                'Insert partial data

                'Dim query As String = "INSERT INTO [customers] (firstname, surname) VALUES ( ' " & firstname & " ', ' " & surname & "');"

                'Execute the built query
                Dim command As New OleDbCommand(query, conn)
                SQLReader = command.ExecuteReader()

                End


            ElseIf NPC2Count = 0 Then
                MsgBox("You lose")

                'Connect to database
                Dim SQLReader As OleDbDataReader
                Dim connection_type As String = "Provider=Microsoft.ACE.OLEDB.12.0;"
                Dim file_location As String = "Data Source=N:\Advanced Computing\AH1\test.accdb"
                Dim conn As OleDbConnection
                conn = New OleDbConnection(connection_type & file_location)
                conn.Open()

                'Convert stored data to SQL query - edited from book!!!
                Dim query As String = "INSERT INTO [Players] VALUES ( 'Null' , '" & NPC2 & "', '1');"

                'Insert partial data

                'Dim query As String = "INSERT INTO [customers] (firstname, surname) VALUES ( ' " & firstname & " ', ' " & surname & "');"

                'Execute the built query
                Dim command As New OleDbCommand(query, conn)
                SQLReader = command.ExecuteReader()

                End


            ElseIf NPC3Count = 0 Then
                MsgBox("You lose")

                'Connect to database
                Dim SQLReader As OleDbDataReader
                Dim connection_type As String = "Provider=Microsoft.ACE.OLEDB.12.0;"
                Dim file_location As String = "Data Source=N:\Advanced Computing\AH1\test.accdb"
                Dim conn As OleDbConnection
                conn = New OleDbConnection(connection_type & file_location)
                conn.Open()

                'Convert stored data to SQL query - edited from book!!!
                Dim query As String = "INSERT INTO [Players] VALUES ( 'Null' , '" & NPC2 & "', '1');"

                'Insert partial data

                'Dim query As String = "INSERT INTO [customers] (firstname, surname) VALUES ( ' " & firstname & " ', ' " & surname & "');"

                'Execute the built query
                Dim command As New OleDbCommand(query, conn)
                SQLReader = command.ExecuteReader()

                End

            ElseIf playerDeckCount = 0 Then
                MsgBox("You win")
                End

            End If
        End If
    End Function

    '------------------------------------------------------------------------------------------
    Private Sub PlayerCard1_Click(sender As Object, e As EventArgs) Handles PlayerCard1.Click

        Dim counter As Integer = 0

        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)

        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If

    End Sub
    Private Sub PlayerCard2_Click(sender As Object, e As EventArgs) Handles PlayerCard2.Click

        Dim counter As Integer = 1
        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If

    End Sub
    Private Sub PlayerCard3_Click(sender As Object, e As EventArgs) Handles PlayerCard3.Click

        Dim counter As Integer = 2
        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If

    End Sub
    Private Sub PlayerCard4_Click(sender As Object, e As EventArgs) Handles PlayerCard4.Click

        Dim counter As Integer = 3
        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If

    End Sub
    Private Sub PlayerCard5_Click(sender As Object, e As EventArgs) Handles PlayerCard5.Click

        Dim counter As Integer = 4
        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If

    End Sub
    Private Sub PlayerCard6_Click(sender As Object, e As EventArgs) Handles PlayerCard6.Click

        Dim counter As Integer = 5
        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If

    End Sub
    Private Sub PlayerCard7_Click(sender As Object, e As EventArgs) Handles PlayerCard7.Click

        Dim counter As Integer = 6
        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If

    End Sub
    Private Sub PlayerCard8_Click(sender As Object, e As EventArgs) Handles PlayerCard8.Click

        Dim counter As Integer = 7
        playCardCard = playerDeck(counter)
        playCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
        If played = True Then
            playerDeck(counter).Addition = ""
            playerDeck(counter).Type = ""
            playerDeck(counter).Colour = ""
            playerDeck(counter).Number = ""
            visible = False
            Call visibility(visible)
            Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
            Call gamePlay(gameFin)
        End If



    End Sub

    '------------------------------------------------------------------------------------------
    'AI PLay
    Private Sub AIPLay1()
        Dim counter As Integer
        deck = NPC1Deck
        Call AIplayCard(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
    End Sub
    Private Sub AIPLay2()
        Dim counter As Integer
        deck = NPC2Deck
        Call AIplayCard2(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
    End Sub
    Private Sub AIPLay3()
        Dim counter As Integer
        deck = NPC3Deck
        Call AIplayCard3(playCardCard, playedCard, counter, played)
        Call displayDecks(playerDeck, NPC1Deck, NPC2Deck, NPC3Deck, playedCard)
    End Sub

    '------------------------------------------------------------------------------------------
    'WRITE TO DATABASE

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        Dim boolea1 As Boolean

        If PlayerDeck1.Visible = True Then
            boolea1 = False
        Else
            boolea1 = True
        End If

        PlayerDeck1.Visible = boolea1
        PlayerDeck2.Visible = boolea1
        PlayerDeck3.Visible = boolea1
        PlayerDeck4.Visible = boolea1
        PlayerDeck5.Visible = boolea1
        PlayerDeck6.Visible = boolea1
        PlayerDeck7.Visible = boolea1
        PlayerDeck8.Visible = boolea1

        playedCardBox.Visible = boolea1

        txtConsole.Visible = boolea1

        Label3.Visible = boolea1
    End Sub
End Class



