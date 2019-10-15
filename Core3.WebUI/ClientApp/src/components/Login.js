import React, { Component } from 'react'
import { Button, Form, FormGroup, Label, Input } from 'reactstrap'

export default class Login extends Component {
    constructor(props){
        super(props);

        this.state = { 
            login: '', 
            password: ''
        }
    }

    onLoginChange = (e) => {
        this.setState({login: e.target.value})
    }

    onPasswordChange = (e) => {
        this.setState({password: e.target.value})
    }

    onSubmit = (e) => {
        console.log(this.state)
        e.preventDefault();
    }

    render (){
        return (
            <Form onSubmit={this.onSubmit}>
                <FormGroup>
                    <Label for="login">Login</Label>
                    <Input type="text" 
                        name="login" 
                        id="login" 
                        placeholder="..." 
                        onChange={this.onLoginChange} />
                </FormGroup>
                <FormGroup>
                    <Label for="password">Password</Label>
                    <Input type="password" 
                        name="password" 
                        id="password" 
                        placeholder="..." 
                        onChange={this.onPasswordChange} />
                </FormGroup>
                <Button>Submit</Button>
            </Form>
        )
    }
}