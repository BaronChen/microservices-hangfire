import * as React from 'react';
import { ILoginProps } from '../login'

import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';

import { debounce } from 'lodash';
import Typography from '@material-ui/core/Typography/Typography';

export interface ILoginFormProps extends ILoginProps {
  onFormChange: (username: string, password: string) => void;
  onLogin: () => void;
}

const decorate = withStyles<"container" | "formControl" | "button" | "pos">((theme: Theme) => ({
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
  pos: {
    marginBottom: 15,
    width: 300
  }
}));

type PropsWithStyles = ILoginFormProps & WithStyles<"container" | "formControl" | "button" | "pos">;

const DecoratedLoginForm = decorate(
  class LoginForm extends React.Component<PropsWithStyles> {

    private usernameInput: any;
    private passwordInput: any;

    private const inputDebounce = debounce((username: string, password: string) => this.props.onFormChange(username, password), 300, { leading: true, trailing: true });

    public render() {
      const { classes } = this.props;

      return (
        <div className={classes.container}>
          <FormControl className={classes.formControl}>
            <InputLabel htmlFor="username">Username</InputLabel>
            <Input id="username" defaultValue={this.props.username} inputRef={this.setUsernameRef.bind(this)} onChange={this.updateForm.bind(this)} />
          </FormControl>
          <FormControl className={classes.formControl}>
            <InputLabel htmlFor="password">Password</InputLabel>
            <Input id="password" type="password" defaultValue={this.props.password} inputRef={this.setPasswordRef.bind(this)} onChange={this.updateForm.bind(this)} />
          </FormControl>
          <Button variant="outlined" color="primary" className={classes.button} onClick={this.loginClicked.bind(this)}>
            Login
          </Button>
          <Typography className={classes.pos} component="caption" align="center" color="secondary">
            For demo purpose, use 'user1' and 'user2' to login. Password is 'password'.
          </Typography>
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
      this.inputDebounce(this.usernameInput.value, this.passwordInput.value);
    }

    public loginClicked() {
      this.props.onLogin();
    }
  }
);

export default DecoratedLoginForm;
