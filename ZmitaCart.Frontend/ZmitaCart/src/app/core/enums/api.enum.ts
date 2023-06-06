export enum Api {
  REGISTER = '/user/register',
  LOGIN = '/user/login',
  LOGOUT = '/user/logout',
  USER_CREDENTIALS_UPDATE = '/user/updateCredentials',
  USER_CREDENTIALS = '/user',
  USER_OFFERS = '/user/offer',

  GET_SUPERIOR_CATEGORIES = '/category/getAllSuperiors',
  GET_SUB_CATEGORIES = '/category/getBySuperiorId',
  GET_SUB_CATEGORIES_FEW = '/category/getFewBySuperiorId',

  OFFER = '/offer',
  OFFER_MAIN = '/offer/byCategoriesNameQuery',
}
