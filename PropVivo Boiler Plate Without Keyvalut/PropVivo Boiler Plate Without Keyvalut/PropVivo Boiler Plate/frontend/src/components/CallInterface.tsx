import React, { useState, useEffect, useRef } from 'react';
import callService, { type IncomingCall } from '../services/callService';
import './CallInterface.css';

const CallInterface: React.FC = () => {
    const [currentCall, setCurrentCall] = useState<IncomingCall | null>(null);
    const [callHistory, setCallHistory] = useState<IncomingCall[]>([]);
    const [isCallActive, setIsCallActive] = useState(false);
    const [testPhoneNumber, setTestPhoneNumber] = useState('+1234567890');
    const [audioStream, setAudioStream] = useState<MediaStream | null>(null);
    const [isVoiceModActive, setVoiceModActive] = useState(false);
    const localRef = useRef<HTMLAudioElement>(null);
    const remoteRef = useRef<HTMLAudioElement>(null);
    let recorder: MediaRecorder | null = null;


    useEffect(() => {
        // Listen for incoming calls
        callService.onIncomingCall((call: IncomingCall) => {
            setCurrentCall(call);
            setCallHistory(prev => [call, ...prev]);
            // Play notification sound
            playNotificationSound();
        });

        callService.onCallStatusUpdate((data) => {
            console.log('Call status update:', data);
        });

        return () => {
            // Cleanup listeners
        };
    }, []);

    const playNotificationSound = () => {
        // Create audio notification
        const audio = new Audio('data:audio/wav;base64,UklGRnoGAABXQVZFZm10IBAAAAABAAEAQB8AAEAfAAABAAgAZGF0YQoGAACBhYqFbF1fdJivrJBhNjVgodDbq2EcBj+a2/LDciUFLIHO8tiJNwgZaLvt559NEAxQp+PwtmMcBjiR1/LMeSwFJHfH8N2QQAoUXrTp66hVFApGn+DyvmwhBiWZyfPMe...');
        audio.play().catch(e => console.log('Audio play failed:', e));
    };

    const handleAnswerCall = async () => {
        setIsCallActive(true);
        const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
        setAudioStream(stream);
        localRef.current!.srcObject = stream;

        recorder = new MediaRecorder(stream);
        recorder.ondataavailable = async (e) => {
            if (isVoiceModActive) {
                const mod = await callService.modulateVoice(
                    new Uint8Array(await e.data.arrayBuffer()),
                    'indian',
                    'american'
                );
                const blob = new Blob([mod], { type: 'audio/webm' });
                remoteRef.current!.src = URL.createObjectURL(blob);
                remoteRef.current!.play();
            }
        };
        recorder.start(1000);

        await callService.startVoiceCall(currentCall!.callId);
        setVoiceModActive(true);
    };

    const handleEndCall = async () => {
        recorder?.stop();
        audioStream?.getTracks().forEach(t => t.stop());
        setIsCallActive(false);
        setCurrentCall(null);
        setVoiceModActive(false);
        await callService.endVoiceCall(currentCall!.callId);
    };


    const simulateCall = async () => {
        try {
            await callService.simulateIncomingCall(testPhoneNumber);
        } catch (error) {
            console.error('Failed to simulate call:', error);
        }
    };

    return (
        <div className="call-interface">
            <audio ref={localRef} autoPlay muted />
            <audio ref={remoteRef} autoPlay />
            <div className="header">
                <h1>Customer Support Portal</h1>
                <div className="test-controls">
                    <input
                        type="text"
                        value={testPhoneNumber}
                        onChange={(e) => setTestPhoneNumber(e.target.value)}
                        placeholder="Phone number"
                    />
                    <button onClick={simulateCall}>Simulate Incoming Call</button>
                </div>
            </div>

            {currentCall && (
                <div className={`incoming-call ${isCallActive ? 'active' : 'incoming'}`}>
                    <div className="call-info">
                        <h2>
                            {isCallActive ? 'Active Call' : 'Incoming Call'}
                        </h2>
                        <div className="caller-details">
                            <div className="phone-number">
                                📞 {currentCall.callerPhoneNumber}
                            </div>
                            {currentCall.customerFound && currentCall.customer ? (
                                <div className="customer-info">
                                    <h3>Customer Information</h3>
                                    <div className="customer-details">
                                        <p><strong>Name:</strong> {currentCall.customer.fullName}</p>
                                        <p><strong>Email:</strong> {currentCall.customer.email}</p>
                                        <p><strong>Company:</strong> {currentCall.customer.company || 'N/A'}</p>
                                        <p><strong>Phone:</strong> {currentCall.customer.phoneNumber}</p>
                                    </div>
                                </div>
                            ) : (
                                <div className="no-customer">
                                    <p>⚠️ Customer not found in database</p>
                                    <p>Phone: {currentCall.callerPhoneNumber}</p>
                                </div>
                            )}
                        </div>
                    </div>

                    <div className="call-controls">
                        {!isCallActive ? (
                            <>
                                <button className="answer-btn" onClick={handleAnswerCall}>
                                    📞 Answer Call
                                </button>
                                <button className="decline-btn" onClick={handleEndCall}>
                                    📞 Decline
                                </button>
                            </>
                        ) : (
                            <>
                                <button className="mute-btn">🔇 Mute</button>
                                <button className="hold-btn">⏸️ Hold</button>
                                <button className="end-btn" onClick={handleEndCall}>
                                    📞 End Call
                                </button>
                            </>
                        )}
                    </div>

                    {isCallActive && (
                        <div className="voice-controls">
                            <div className="voice-status">
                                <p>🎤 Voice Modulation: Indian → American</p>
                                <div className="audio-visualizer">
                                    <div className="wave"></div>
                                    <div className="wave"></div>
                                    <div className="wave"></div>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            )}

            <div className="call-history">
                <h3>Recent Calls</h3>
                {callHistory.length === 0 ? (
                    <p>No recent calls</p>
                ) : (
                    <ul>
                        {callHistory.slice(0, 10).map((call) => (
                            <li key={call.callId}>
                                <span>{call.callerPhoneNumber}</span>
                                <span>{call.customerFound ? call.customer?.fullName : 'Unknown'}</span>
                                <span>{new Date(call.callTime).toLocaleTimeString()}</span>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );
};

export default CallInterface;
