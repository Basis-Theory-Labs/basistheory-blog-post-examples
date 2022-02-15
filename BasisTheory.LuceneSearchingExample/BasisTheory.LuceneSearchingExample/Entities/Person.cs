using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasisTheory.LuceneSearchingExample.entities;

public class Person
{
    [Key] public Guid Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Ssn { get; set; }

    public string FavoriteFood { get; set; }

    private static List<Person> _persons = new()
    {
        new Person
        {
            Id = new Guid("57cd7bef-2b3f-4c0e-a4f6-0cbfaff5bca7"),
            FirstName = "Bob",
            LastName = "Hudec",
            Ssn = "123-45-6789",
            FavoriteFood = "Pizza"
        },
        new Person
        {
            Id = new Guid("35845407-393a-4c48-8ebb-5f0fedc932c2"),
            FirstName = "James",
            LastName = "Weber",
            Ssn = "123-45-1234",
            FavoriteFood = "Hamburger with fries"
        },
        new Person
        {
            Id = new Guid("e57aae0f-7b5d-4fba-b509-7f1b3a06e326"),
            FirstName = "Colin",
            LastName = "Aquino",
            Ssn = "123-21-6782",
            FavoriteFood = "Steak"
        },
        new Person
        {
            Id = new Guid("c673f80b-e496-4c99-b469-44a8f29352b7"),
            FirstName = "Drew",
            LastName = "Letizia",
            Ssn = "123-67-2189",
            FavoriteFood = "Hamburger with fries"
        },
        new Person
        {
            Id = new Guid("3bafb06a-7422-4b8e-8772-cfaaee10af1b"),
            FirstName = "Adam",
            LastName = "Gonzalez",
            Ssn = "131-87-6789",
            FavoriteFood = "Pizza"
        },
        new Person
        {
            Id = new Guid("c673f80b-7422-4b8e-8772-7f1b3a06e326"),
            FirstName = "Thunder",
            LastName = "Lampe",
            Ssn = "123-45-5421",
            FavoriteFood = "Pizza"
        }
    };

    internal static void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasData(_persons);
    }
}