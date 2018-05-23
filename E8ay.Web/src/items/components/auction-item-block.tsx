import * as React from 'react';
import { IAuctionItem, ItemStatus } from '../models'

import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';

import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { debounce } from 'lodash';

import { format, parse } from 'date-fns';
import { Countdown } from '../../common/countdown/coutdown.component';

export interface IAuctionItemProps {
  auctionItem: IAuctionItem,
  currentUserId: string,
  onBidPriceUpdate: (bidPrice: number) => void;
  onPlaceBid: () => void;
}

const decorate = withStyles<"card" | "pos" | "textField">((theme: Theme) => ({
  card: {
    minWidth: 200,
    height: '100%'
  },
  pos: {
    marginBottom: 12,
  },
  textField: {
    width: 150,
  },
}));

type PropsWithStyles = IAuctionItemProps & WithStyles<"card" | "pos" | "textField">;

const DecoratedAuctionItemBlock = decorate(
  class AuctionItemBlock extends React.Component<PropsWithStyles> {

    private const bidPriceDebounce = debounce((bidPrice: number) => this.props.onBidPriceUpdate(bidPrice), 300, { leading: true, trailing: true });

    public bidPriceChanged(event: any) {
      this.bidPriceDebounce(Number(event.target.value));
    }

    public placeBidClicked() {
      this.props.onPlaceBid();
    }

    public render() {
      const { auctionItem, classes, currentUserId } = this.props;

      return (
        <Card className={classes.card}>
          <CardContent>
            <Typography variant="headline" component="h2">
              {auctionItem.name}
            </Typography>
            <Typography className={classes.pos} color="error">
              Current Price: ${auctionItem.highestPrice}
            </Typography>
            {
              currentUserId === auctionItem.highestBiderId ?
                <Typography className={classes.pos} color="error" variant="subheading">
                  You are the highest bidder for now.     
                </Typography>
                :
                null
            }

            <Typography className={classes.pos} component="p">
              {auctionItem.description}
            </Typography>

            <Typography variant="subheading">
              Start:
            </Typography>
            <Typography variant="body2">
              {format(parse(auctionItem.startDateTime), 'DD/MM/YYYY HH:mm:ss')}
            </Typography>

            <Typography variant="subheading">
              Ends In:
            </Typography>
            <Typography variant="body2" color="secondary">
              <Countdown dateStr={auctionItem.endDateTime} />
            </Typography>

            {
              auctionItem.status !== ItemStatus.Sold && auctionItem.status !== ItemStatus.End ?
                <TextField
                  id="number"
                  label="Your Price"
                  onChange={this.bidPriceChanged.bind(this)}
                  type="number"
                  className={classes.textField}
                  InputLabelProps={{
                    shrink: true,
                  }}
                  InputProps={{ inputProps: { min: 0, max: 9999999999 } }}
                  margin="normal"
                />
                :
                <Typography component="h3" color="error">
                  {this.getAuctionEndedMessage(auctionItem.status)}
                </Typography>
            }

          </CardContent>
          {
            auctionItem.status !== ItemStatus.Sold && auctionItem.status !== ItemStatus.End ?
              <CardActions>
                <Button color="primary" size="small" onClick={this.placeBidClicked.bind(this)}>Bid</Button>
              </CardActions>
              :
              null
          }

        </Card>
      )
    }

    private getAuctionEndedMessage(status: ItemStatus) {
      return status === ItemStatus.End ? "Auction has ended" : "Item has been sold";
    }

  }
);

export default DecoratedAuctionItemBlock;
