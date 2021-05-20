using System;
using auth_cs_gregslist.Models;
using auth_cs_gregslist.Repositories;

namespace auth_cs_gregslist.Services
{
  public class AccountsService
  {
    private readonly AccountsRepository _repo;

    public AccountsService(AccountsRepository repo)
    {
      _repo = repo;
    }

    internal Account GetOrCreateAccount(Account userInfo)
    {
      Account account = _repo.GetById(userInfo.Id);
      if (account == null)
      {
        return _repo.Create(userInfo);
      }
      return account;
    }
  }
}