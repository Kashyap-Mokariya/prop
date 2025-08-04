import * as signalR from '@microsoft/signalr';
import axios from 'axios';

export interface Customer {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  company?: string;
  fullName: string;
}

export interface IncomingCall {
  callId: string;
  customer?: Customer;
  callerPhoneNumber: string;
  customerFound: boolean;
  callTime: string;
}

class CallService {
  private connection: signalR.HubConnection;
  private readonly baseUrl = 'https://localhost:7266/api/v1';

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7266/callhub')
      .build();

    this.startConnection();
  }

  private async startConnection() {
    try {
      await this.connection.start();
      console.log('SignalR Connected');
    } catch (err) {
      console.error('SignalR Connection Error: ', err);
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  onIncomingCall(callback: (call: IncomingCall) => void) {
    this.connection.on('IncomingCall', callback);
  }

  onCallStatusUpdate(callback: (data: { callId: string; status: string }) => void) {
    this.connection.on('CallStatusUpdate', callback);
  }

  async simulateIncomingCall(phoneNumber: string) {
    try {
      const response = await axios.post(`${this.baseUrl}/call/incoming`, {
        callerPhoneNumber: phoneNumber,
        receiverPhoneNumber: '+1555000123',
        callTime: new Date().toISOString(),
        executionContext: {
          trackingId: crypto.randomUUID(),
          sessionId: 'demo-session'
        }
      });
      return response.data;
    } catch (error) {
      console.error('Error simulating call:', error);
      throw error;
    }
  }
}

export default new CallService();
