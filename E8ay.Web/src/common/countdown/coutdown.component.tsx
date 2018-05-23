import * as React from 'react';
import { parse } from 'date-fns';

export interface ICountdownProps {
  dateStr: string
}

export interface ICountdownState {
  seconds: number
}

export class Countdown extends React.Component<ICountdownProps, ICountdownState> {

  constructor(props:ICountdownProps) {
    super(props);
    const now = new Date();
    const date = parse(this.props.dateStr);
    let seconds = (date.getTime() - now.getTime()) / 1000;
    seconds = seconds < 0 ? 0 : seconds;

    this.state = {seconds};
    setInterval(() => {
      this.timer()
    }, 1000);
  }

  public render() {
    const {seconds } = this.state;
    const day = Math.floor(seconds / 86400);
    const minutes = Math.floor((seconds % 86400) / 60);
    const remainSeconds = Math.floor(seconds % 60);
    return (
      <span>
        {day} Day {minutes} minutes {remainSeconds} seconds
      </span>
    );
  }

  private timer(): void{
    const nextSeconds = this.state.seconds - 1;
    if (nextSeconds > 0) {
      this.setState({
        seconds: nextSeconds
      })
    }
  }
}