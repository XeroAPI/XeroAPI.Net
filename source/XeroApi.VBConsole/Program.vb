Module Program

    Sub Main()

        Console.WriteLine("Do you want to run as a public or private application?")
        Console.WriteLine(" Press 1 for a public application")
        'Console.WriteLine(" Press 2 for a private application")
        'Console.WriteLine(" Press 3 for a partner application")

        Dim keyInfo As ConsoleKeyInfo = Console.ReadKey(True)
        Console.WriteLine()


        If (keyInfo.KeyChar.Equals("1"c)) Then

            Console.WriteLine("Running as a public application...")
            ExerciseOrganisation(PublicApplicationRunner.CreateRepository())

        End If


        'if (keyInfo.KeyChar = '2')

        '    Console.WriteLine("Running as a private application...")
        '    ExerciseOrganisation(PrivateApplicationRunner.CreateRepository())

        'End If

        '    if (keyInfo.KeyChar = '3')

        '    Console.WriteLine("Running as a partner application...")
        '    ExerciseOrganisation(PartnerApplicationRunner.CreateRepository())

        'End If

        Console.WriteLine("")
        Console.WriteLine(" Press Enter to Exit")
        Console.ReadLine()

    End Sub

    Sub ExerciseOrganisation(repository As Repository)

        Dim organisation As XeroApi.Model.Organisation = repository.Organisation
        Console.WriteLine("You are currently connected to {0}", organisation.Name)

    End Sub

End Module
