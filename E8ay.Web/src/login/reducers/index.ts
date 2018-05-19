
import { combineReducers } from 'redux';
import { createSelector } from 'reselect'

import { actionTypes, IUpdateLoginForm, LoginActions, ISubmitLogin  } from '../actions/login.actions';
import { IJwt } from '../models';
import { IRootState } from '../../store';

import { handle } from 'redux-pack';


export interface ILoginState {
  readonly loginForm: {
    readonly username: string,
    readonly password: string
  },
  readonly auth: IJwt
};

export const loginReducer = combineReducers<ILoginState, LoginActions>({
  auth: (state = {id:'', auth_token: '', expires_in: 0}, action) => {
    switch (action.type) {
      // case actionTypes.UPDATE_AUTH_TOKEN:
      //   return Object.assign({}, state, (action as IUpdateAuthToken).payload);
      case actionTypes.SUBMIT_LOGIN:
        return handle(state, action, {
          success: prevState => Object.assign({}, prevState, (action as ISubmitLogin).payload)
        });
      default: 
        return state;
    }
  },
  loginForm: (state = {username:'', password:''}, action) => {
    switch (action.type) {
      case actionTypes.UPDATE_LOGIN_FORM:
        return Object.assign({}, state, {username: (action as IUpdateLoginForm).username, password: (action as IUpdateLoginForm).password})
      default: 
        return state;
    }
  }
});

export const getLogin = (state: IRootState):ILoginState => state.login;
export const getUsername = createSelector<IRootState, ILoginState, string>(
  getLogin,
  (state:ILoginState) => state.loginForm.username
);

export const getPassword = createSelector<IRootState, ILoginState, string>(
  getLogin,
  (state:ILoginState) => state.loginForm.password
);

export const getAuthToken = createSelector<IRootState, ILoginState, string>(
  getLogin,
  (state:ILoginState) => state.auth.auth_token
);
