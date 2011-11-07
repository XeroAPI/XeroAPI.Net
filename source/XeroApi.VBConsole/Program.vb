Imports XeroApi.Model

Module Program

    Sub Main()

        Console.WriteLine("Do you want to run as a public or private application?")
        Console.WriteLine(" Press 1 for a public application")
        Console.WriteLine(" Press 2 for a private application")
        'Console.WriteLine(" Press 3 for a partner application")

        Dim keyInfo As ConsoleKeyInfo = Console.ReadKey(True)
        Console.WriteLine()


        If (keyInfo.KeyChar.Equals("1"c)) Then

            Console.WriteLine("Running as a public application...")
            ExerciseOrganisation(PublicApplicationRunner.CreateRepository())

        ElseIf (keyInfo.KeyChar.Equals("2"c)) Then

            Console.WriteLine("Running as a private application...")
            ExerciseOrganisation(PrivateApplicationRunner.CreateRepository())

            'ElseIf (keyInfo.KeyChar = '3') Then

            '     Console.WriteLine("Running as a partner application...")
            '    ExerciseOrganisation(PartnerApplicationRunner.CreateRepository())

        End If

        Console.WriteLine("")
        Console.WriteLine(" Press Enter to Exit")
        Console.ReadLine()

    End Sub

    Sub ExerciseOrganisation(repository As Repository)

        Dim organisation As XeroApi.Model.Organisation = repository.Organisation
        Console.WriteLine("You are currently connected to {0}", organisation.Name)

        ' Q39411
        Dim arInvoices = From invoices In repository.Invoices Where invoices.Type = "ACCREC"

        For Each arInvoice As Invoice In arInvoices
            Console.WriteLine("Invoice {0} is at status {1}", arInvoice.InvoiceNumber, arInvoice.Status)
        Next


        Dim notArInvoices = From invoices In repository.Invoices Where invoices.Type <> "ACCREC"

        For Each notAnArInvoice As Invoice In notArInvoices
            Console.WriteLine("Invoice {0} is at status {1}", notAnArInvoice.InvoiceNumber, notAnArInvoice.Status)
        Next

    End Sub

End Module
