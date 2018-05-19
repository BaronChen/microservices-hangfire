import * as React from 'react';
import { ILoginProps } from '../login'

import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';

import * as lodash from 'lodash';

export interface ILoginFormProps extends ILoginProps {
  onFormChange: (username: string, password: string) => void;
  onLogin: () => void;
}

const decorate = withStyles<"container" | "formControl" | "button">((theme: Theme) => ({
  button: {
    margin: theme.spacing.unit,
  },
  container: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center'
  },
  formControl: {
    margin: theme.spacing.unit
  },
}));

type PropsWithStyles = ILoginFormProps & WithStyles<"container" | "formControl" | "button">;

const DecoratedLoginForm = decorate(
  class LoginForm extends React.Component<PropsWithStyles> {

    private usernameInput: any;
    private passwordInput: any;

    private const debounce =  lodash.debounce((username: string, password: string) => this.props.onFormChange(username, password), 300, {leading:true, trailing: true});

    public render() {
      const { classes } = this.props;

      return (
        <div className={classes.container}>
          <FormControl className={classes.formControl}>
            <InputLabel htmlFor="username">Username</InputLabel>
            <Input id="username" defaultValue={this.props.username} inputRef={this.setUsernameRef.bind(this)} onChange={this.updateForm.bind(this)}/>
          </FormControl>
          <FormControl className={classes.formControl}>
            <InputLabel htmlFor="password">Password</InputLabel>
            <Input id="password" type="password" defaultValue={this.props.password} inputRef={this.setPasswordRef.bind(this)} onChange={this.updateForm.bind(this)}/>
          </FormControl>
          <Button variant="outlined" color="primary" className={classes.button} onClick={this.loginClicked.bind(this)}>
            Login
          </Button>
        </div>
      );
    }

    public const setUsernameRef(el: HTMLElement) {
      this.usernameInput = el;
    }

    public const setPasswordRef(el: HTMLElement) {
      this.passwordInput = el;
    }
  
    public updateForm() {
      this.debounce(this.usernameInput.value, this.passwordInput.value);
    }

    public loginClicked() {
      this.props.onLogin();
    }
  }
);

export default DecoratedLoginForm;
