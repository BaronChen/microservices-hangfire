import { Action, Dispatch } from 'redux';
import { login } from '../login.api';
import { IJwt } from '../models';
import { IRootState } from '../../reducer';

import { GetState, ActionMeta } from 'redux-pack';

import { push } from 'react-router-redux';

export const actionTypes = {
  SUBMIT_LOGIN: '[LOGIN]SUBMIT_LOGIN',
  UPDATE_AUTH_TOKEN: '[LOGIN]UPDATE_AUTH_TOKEN',
  UPDATE_LOGIN_FORM: '[LOGIN]UPDATE_LOGIN_FORM'
}


export interface IUpdateLoginForm extends Action {
  username : string, 
  password : string
}

export interface ISubmitLogin extends Action {
  payload?: IJwt,
  promise: Promise<any>,
  meta: ActionMeta
}

export type LoginActions = IUpdateLoginForm | ISubmitLogin| Action;

export const updateLoginForm = (username:string, password:string): IUpdateLoginForm => ({
  password,
  type: actionTypes.UPDATE_LOGIN_FORM,
  username
})

export const submitLogin = (username:string, password:string, dispatch: Dispatch): ISubmitLogin => ({
  type: actionTypes.SUBMIT_LOGIN,
  promise: login(username, password),
  meta: {
    onFailure: (error: string, getState: GetState<IRootState>) => {
      alert(error);
    },
    onSuccess: (response: IJwt, getState: GetState<IRootState>) => {
      dispatch(push('/items'));
    }
  }
})

