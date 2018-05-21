import { routerReducer, RouterState} from 'react-router-redux';
import { combineReducers } from 'redux';
import { ILoginState, loginReducer  } from './login/reducers';


export interface IRootState {
  login: ILoginState;
  router: RouterState;
}


export const rootReducer = combineReducers<IRootState>({
  login: loginReducer,
  router: routerReducer
});
