import { useAuth } from './AuthContext';
import './index.css'
import Login from './components/Login';



function App() {
 const { token } = useAuth();
return (!token && < Login />)
}

export default App
