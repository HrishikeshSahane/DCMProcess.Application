import {
    TokenCredential,
    GetTokenOptions,
    AccessToken,
  } from '@azure/core-http';
  
  export class MyCredential implements TokenCredential {
    private tokens: string;
    constructor(token: string) {
      this.tokens = token;
    }
    public async getToken(
      scopes: string | string[],
      options?: GetTokenOptions
    ): Promise<AccessToken> {
      var result = new MyToken(this.tokens);
  
      console.log(result);
      return result;
    }
  }
  
  class MyToken implements AccessToken {
    token: string;
    expiresOnTimestamp: number;
  
    constructor(token: string) {
      this.token = token;
    }
  }