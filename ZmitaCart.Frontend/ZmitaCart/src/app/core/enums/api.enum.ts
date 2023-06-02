export enum Api {
  REGISTER = '/user/register',
  LOGIN = '/user/login',
  LOGOUT = '/user/logout',

  GET_SUPERIOR_CATEGORIES = '/category/getAllSuperiors',
  GET_SUB_CATEGORIES = '/category/getBySuperiorId',
  GET_SUB_CATEGORIES_FEW = '/category/getFewBySuperiorId',
}
