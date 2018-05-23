import { routerReducer, RouterState} from 'react-router-redux';
import { combineReducers } from 'redux';
import { ILoginState, loginReducer  } from './login/reducers';
import { IItemsState, itemsReducer } from './items/reducers';

export interface IRootState {
  login: ILoginState;
  router: RouterState;
  items: IItemsState
}

export const rootReducer = combineReducers<IRootState>({
  login: loginReducer,
  router: routerReducer,
  items: itemsReducer
});
