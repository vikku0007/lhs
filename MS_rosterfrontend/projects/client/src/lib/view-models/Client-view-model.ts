import { AddClientDetails } from './add-client-details';
import { ClientOnBoardinNotes } from './client-onboardingnotes';
import { ClientAdditionalNotes } from './client-additionalnotes';
import { ClientFundingInfo } from './client-fundinginfo';
import { ClientPrimarycarerInfo } from './client-primary-carerinfo';



export interface ClientViewModel {
    ClientPrimaryInfo?: AddClientDetails;
    ClientPrimaryCareInfo?: ClientPrimarycarerInfo;
    clientOnboadingInfo?: ClientOnBoardinNotes;
    ClientAdditionalNotes?: ClientAdditionalNotes;
    ClientFunding?: null;
    ClientFundingInfo?: ClientFundingInfo;
    
}