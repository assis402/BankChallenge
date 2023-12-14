using BankChallenge.Business.Enums;
using BankChallenge.Business.Helpers;
using BankChallenge.Shared.Dtos.AccountHolder;
using BankChallenge.Shared.Dtos.Identity;
using BankChallenge.Shared.Helpers;
using Matsoft.MongoDB;
using Matsoft.MongoDB.CustomAttributes;

namespace BankChallenge.Business.Entities;

[CollectionName("accountHolder")]
public class AccountHolderEntity : BaseEntity
{
    public AccountHolderEntity(SignUpDto signUpDto)
    {
        Email = signUpDto.Email;
        Password = CryptographyMd5.Encrypt(signUpDto.Password);
        Cpf = signUpDto.Cpf.RemoveNonNumeric();
        Name = signUpDto.Name;
        Birthdate = signUpDto.Birthdate;
        Address = signUpDto.Address;
        Status = AccountHolderStatus.Active;
    }

    public static implicit operator AccountHolderDto(AccountHolderEntity accountHolderEntity)
        => new(accountHolderEntity.Name, accountHolderEntity.Email);

    public string Email { get; private set; }

    public string Password { get; private set; }

    public string Cpf { get; private set; }

    public string Name { get; private set; }

    public DateOnly Birthdate { get; private set; }

    public string Address { get; private set; }

    public AccountHolderStatus Status { get; private set; }
}