import * as React from 'react';
import { connect } from 'react-redux';

import { IRootState } from '../reducer';
import { getPassword, getUsername } from './reducers';

import { Dispatch } from 'redux';

import { updateLoginForm, submitLogin } from './actions/login.actions';
import LoginForm from './components/login-form';

export interface ILoginProps {
  username: string,
  password: string,
}

export class Login extends React.Component<ILoginProps & {dispatch: Dispatch}> {
  public render() {
    return (
      <div>
        <LoginForm username={this.props.username} password={this.props.password} onFormChange={this.updateForm.bind(this)} onLogin={this.login.bind(this)}/>
      </div>
    );
  }

  public updateForm(username:string, password:string) {
    this.props.dispatch(updateLoginForm(username, password));
  }

  public login() {
    this.props.dispatch(submitLogin(this.props.username, this.props.password, this.props.dispatch));
  }
}

const mapStateToProps = (state: IRootState):ILoginProps => ({
  password: getPassword(state),
  username: getUsername(state)
});

export default connect(mapStateToProps)(Login);
